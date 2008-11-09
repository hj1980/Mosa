﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem.ISA
{
    /// <summary>
    /// 
    /// </summary>
	public class Registry
	{
        /// <summary>
        /// 
        /// </summary>
		protected PlatformArchitecture platformArchitecture;

        /// <summary>
        /// 
        /// </summary>
		protected LinkedList<DriverEntry> drivers;

		/// <summary>
		/// Initializes a new instance of the <see cref="Registry"/> class.
		/// </summary>
		/// <param name="platformArchitecture">The platform architecture.</param>
		public Registry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			drivers = new LinkedList<DriverEntry>();
		}

        /// <summary>
        /// Adds the device driver.
        /// </summary>
        /// <param name="deviceDriverSignature">The device driver signature.</param>
        /// <param name="type">The type.</param>
		public void AddDeviceDriver(DeviceSignatureAttribute deviceDriverSignature, System.Type type)
		{
			drivers.Add(new DriverEntry(deviceDriverSignature, type));
		}

        /// <summary>
        /// Registers the build in device drivers.
        /// </summary>
		public void RegisterBuildInDeviceDrivers()
		{
			System.Reflection.Assembly assemblyInfo = typeof(Registry).Module.Assembly;
			RegisterDeviceDrivers(assemblyInfo);
		}

		/// <summary>
		/// Registers the device drivers.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public void RegisterDeviceDrivers(System.Reflection.Assembly assembly)
		{
			System.Type[] types = assembly.GetTypes();

			foreach (System.Type type in types) {
				object[] attributes = type.GetCustomAttributes(typeof(DeviceSignatureAttribute), false);

				foreach (object attribute in attributes)
					if (((attribute as DeviceSignatureAttribute).Platforms & platformArchitecture) != 0)
						AddDeviceDriver(attribute as DeviceSignatureAttribute, type);
			}
		}

        /// <summary>
        /// Starts the drivers.
        /// </summary>
        /// <param name="deviceManager">The device manager.</param>
        /// <param name="resourceManager">The resource manager.</param>
		public void StartDrivers(IDeviceManager deviceManager, IResourceManager resourceManager)
		{
			foreach (DriverEntry entry in drivers) {
				if (entry.SignatureAttribute.AutoLoad) {
					IIOPortRegion[] ioPortRegions;
					IMemoryRegion[] memoryRegion;

					if (entry.SignatureAttribute.BasePort != 0x0) {
						if (entry.SignatureAttribute.AltBasePort != 0x0) {
							ioPortRegions = new IOPortRegion[2];
							ioPortRegions[1] = new IOPortRegion(entry.SignatureAttribute.AltBasePort, entry.SignatureAttribute.AltPortRange);
						}
						else {
							ioPortRegions = new IOPortRegion[1];
						}
						ioPortRegions[0] = new IOPortRegion(entry.SignatureAttribute.BasePort, entry.SignatureAttribute.PortRange);
					}
					else {
						ioPortRegions = new IOPortRegion[0];
					}

					if (entry.SignatureAttribute.BaseAddress != 0x0) {
						memoryRegion = new MemoryRegion[1];
						memoryRegion[0] = new MemoryRegion(entry.SignatureAttribute.BaseAddress, entry.SignatureAttribute.AddressRange);
					}
					else {
						memoryRegion = new MemoryRegion[0];
					}

					ISAHardwareDevice isaHardwareDevice = System.Activator.CreateInstance(entry.DriverType) as ISAHardwareDevice;
					IBusResources busResources = new BusResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, entry.SignatureAttribute.IRQ, isaHardwareDevice));

					isaHardwareDevice.AssignBusResources(busResources);

					deviceManager.Add(isaHardwareDevice);

					isaHardwareDevice.Activate(deviceManager);
				}
			}
		}
	}
}
