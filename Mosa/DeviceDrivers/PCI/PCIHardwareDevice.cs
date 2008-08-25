﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers.PCI
{

	public abstract class PCIHardwareDevice : HardwareDevice
	{
		protected BusResources busResources;

		public PCIHardwareDevice(PCIDevice pciDevice) : base() { base.parent = (IDevice)pciDevice; }

		public void AssignResources(BusResources busResources)
		{
			this.busResources = busResources;
		}

	}

}
