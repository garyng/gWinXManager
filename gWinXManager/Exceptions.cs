using System;
using System.Collections.Generic;
using System.Linq;

namespace gWinXManager
{
	static class Exceptions
	{
		public static Exception PathNotFound = new Exception("Specific file does not exist.");
		public static Exception GetArgsFailed = new Exception("Failed to retrieve target's arguments.");
		public static Exception GetDesFailed = new Exception("Failed to retrieve target description.");
		public static Exception GetIconLocFailed = new Exception("Failed to retrieve target's icon location.");
		public static Exception PropertyStoreFailed = new Exception("Failed to get property store.");
		public static Exception PropertySetFailed = new Exception("Failed to set property store value.");
		public static Exception PropertyCommitFailed = new Exception("Failed to write changes to .lnk.");
		public static Exception SetTargetFailed = new Exception("Failed to set shortcut's target path.");
		public static Exception SetDesFailed = new Exception("Failed to set shortcut's description.");
		public static Exception SetArgFailed = new Exception("Failed to set shortcut's arguments.");

		public static Exception GroupNotVaild = new Exception("Not a valid group name.");
		public static Exception GroupExist = new Exception("Group with same name exist!");

		public static Exception HashFailed = new Exception("Failed to hash data.");
	}
}
