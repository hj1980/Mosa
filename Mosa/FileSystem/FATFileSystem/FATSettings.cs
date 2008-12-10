﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.FileSystem.FATFileSystem
{
    /// <summary>
    /// 
    /// </summary>
	public class FATSettings : SettingsBase
	{
		/// <summary>
		/// 
		/// </summary>
		public FATType FATType;

		/// <summary>
		/// 
		/// </summary>
		public byte[] SerialID;

		/// <summary>
		/// 
		/// </summary>
		public bool FloppyMedia;

		/// <summary>
		/// Initializes a new instance of the <see cref="FATSettings"/> class.
		/// </summary>
		public FATSettings()
		{
			this.FATType = FATType.FAT16;	// default
			this.SerialID = new byte[0];
			this.FloppyMedia = false;
		}
	}
}