﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.FileSystem;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FATFileSystem
{
	public class VFSDirectory : DirectoryNode
	{
		protected uint directoryCluster;

		public VFSDirectory(IFileSystem fs, uint directoryCluster)
			: base(fs)
		{
			this.directoryCluster = directoryCluster;
		}

		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			return null;
		}

		public override IVfsNode Lookup(string name)
		{
			FAT.DirectoryEntryLocation location = (FileSystem as VFSFileSystem).FAT.FindEntry(new FAT.FatEntityComparer(name), this.directoryCluster);

			if (!location.Valid)
				return null;

			if (location.IsDirectory)
				return new VFSDirectory(FileSystem, location.Block);
			else
				return new VFSFile(FileSystem, location.Block, location.DirectorySector, location.DirectoryIndex);
		}

		public override object Open(System.IO.FileAccess access, System.IO.FileShare share)
		{
			return null;
		}

		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			FAT fs = this.FileSystem as FAT;

			uint childBlock = (child as VFSDirectory).directoryCluster;

			FAT.DirectoryEntryLocation location = fs.FindEntry(new FAT.FatMatchClusterComparer(childBlock), this.directoryCluster);

			if (!location.Valid)
				throw new System.ArgumentException(); //throw new IOException ("Unable to delete directory because it is not empty");

			fs.Delete(childBlock, directoryCluster, location.DirectoryIndex);
		}

	}
}