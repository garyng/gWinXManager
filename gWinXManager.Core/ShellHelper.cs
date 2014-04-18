// Declaration of Structs, Enums, Interfaces were collected by GaryNg@http://garyngzhongbo.blogspot.com
// I spent almost 2 days on this, searching at p/invoke, GitHub, Google Code and many more.
// Keep these lines if you are using it!
// Thanks!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace gWinXManager.Core
{
	namespace ShellHelper
	{
		#region Const

		public static class STGM
		{
			public const int STGM_READ = 0x0;
			public const int STGM_WRITE = 0x1;
			public const int STGM_READWRITE = 0x2;
			public const int STGM_SHARE_DENY_NONE = 0x40;
			public const int STGM_SHARE_DENY_READ = 0x30;
			public const int STGM_SHARE_DENY_WRITE = 0x20;
			public const int STGM_SHARE_EXCLUSIVE = 0x10;
			public const int STGM_PRIORITY = 0x40000;
			public const int STGM_CREATE = 0x1000;
			public const int STGM_CONVERT = 0x20000;
			public const int STGM_FAILIFTHERE = 0x0;
			public const int STGM_DIRECT = 0x0;
			public const int STGM_TRANSACTED = 0x10000;
			public const int STGM_NOSCRATCH = 0x100000;
			public const int STGM_NOSNAPSHOT = 0x200000;
			public const int STGM_SIMPLE = 0x8000000;
			public const int STGM_DIRECT_SWMR = 0x400000;
			public const int STGM_DELETEONRELEASE = 0x400000;
		}

		/// <summary>IShellLink.GetPath fFlags: Flags that specify the type of path information to retrieve</summary>
		public static class SLGP_FLAGS
		{
			/// <summary>Retrieves the standard short (8.3 format) file name</summary>
			public const int SLGP_SHORTPATH = 0x1;
			/// <summary>Retrieves the Universal Naming Convention (UNC) path name of the file</summary>
			public const int SLGP_UNCPRIORITY = 0x2;
			/// <summary>Retrieves the raw path name. A raw path is something that might not exist and may include environment variables that need to be expanded</summary>
			public const int SLGP_RAWPATH = 0x4;
		}

		public static class SHGFI
		{
			public const uint SHGFI_ICON = 0x000000100;
			public const uint SHGFI_DISPLAYNAME = 0x000000200;
			public const uint SHGFI_TYPENAME = 0x000000400;
			public const uint SHGFI_ATTRIBUTES = 0x000000800;
			public const uint SHGFI_ICONLOCATION = 0x000001000;
			public const uint SHGFI_EXETYPE = 0x000002000;
			public const uint SHGFI_SYSICONINDEX = 0x000004000;
			public const uint SHGFI_LINKOVERLAY = 0x000008000;
			public const uint SHGFI_SELECTED = 0x000010000;
			public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;
			public const uint SHGFI_LARGEICON = 0x000000000;
			public const uint SHGFI_SMALLICON = 0x000000001;
			public const uint SHGFI_OPENICON = 0x000000002;
			public const uint SHGFI_SHELLICONSIZE = 0x000000004;
			public const uint SHGFI_PIDL = 0x000000008;
			public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
			public const uint SHGFI_ADDOVERLAYS = 0x000000020;
			public const uint SHGFI_OVERLAYINDEX = 0x00000004;
		}

		#endregion

		#region Struct

		[StructLayout(LayoutKind.Explicit, Size = 520)]
		public struct STRRETinternal
		{
			[FieldOffset(0)]
			public IntPtr pOleStr;

			[FieldOffset(0)]
			public IntPtr pStr;  // LPSTR pStr;   NOT USED

			[FieldOffset(0)]
			public uint uOffset;

		}

		[StructLayout(LayoutKind.Sequential)]
		public struct STRRET
		{
			public uint uType;
			public STRRETinternal data;
		}

		// WIN32_FIND_DATAW Structure
		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
		public struct WIN32_FIND_DATAW
		{
			public uint dwFileAttributes;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = APIs.MAX_PATH)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		#endregion

		#region Enum

		public enum SIGDN : uint
		{
			NORMALDISPLAY = 0,
			PARENTRELATIVEPARSING = 0x80018001,
			PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
			DESKTOPABSOLUTEPARSING = 0x80028000,
			PARENTRELATIVEEDITING = 0x80031001,
			DESKTOPABSOLUTEEDITING = 0x8004c000,
			FILESYSPATH = 0x80058000,
			URL = 0x80068000
		}

		[Flags]
		public enum SFGAO : uint
		{
			SFGAO_CANCOPY = 0x1,                // Objects can be copied    (DROPEFFECT_COPY)
			SFGAO_CANMOVE = 0x2,                // Objects can be moved     (DROPEFFECT_MOVE)
			SFGAO_CANLINK = 0x4,                // Objects can be linked    (DROPEFFECT_LINK)
			SFGAO_STORAGE = 0x00000008,         // supports BindToObject(IID_IStorage)
			SFGAO_CANRENAME = 0x00000010,         // Objects can be renamed
			SFGAO_CANDELETE = 0x00000020,         // Objects can be deleted
			SFGAO_HASPROPSHEET = 0x00000040,         // Objects have property sheets
			SFGAO_DROPTARGET = 0x00000100,         // Objects are drop target
			SFGAO_CAPABILITYMASK = 0x00000177,
			SFGAO_ENCRYPTED = 0x00002000,         // object is encrypted (use alt color)
			SFGAO_ISSLOW = 0x00004000,         // 'slow' object
			SFGAO_GHOSTED = 0x00008000,         // ghosted icon
			SFGAO_LINK = 0x00010000,         // Shortcut (link)
			SFGAO_SHARE = 0x00020000,         // shared
			SFGAO_READONLY = 0x00040000,         // read-only
			SFGAO_HIDDEN = 0x00080000,         // hidden object
			SFGAO_DISPLAYATTRMASK = 0x000FC000,
			SFGAO_FILESYSANCESTOR = 0x10000000,         // may contain children with SFGAO_FILESYSTEM
			SFGAO_FOLDER = 0x20000000,         // support BindToObject(IID_IShellFolder)
			SFGAO_FILESYSTEM = 0x40000000,         // is a win32 file system object (file/folder/root)
			SFGAO_HASSUBFOLDER = 0x80000000,         // may contain children with SFGAO_FOLDER
			SFGAO_CONTENTSMASK = 0x80000000,
			SFGAO_VALIDATE = 0x01000000,         // invalidate cached information
			SFGAO_REMOVABLE = 0x02000000,         // is this removeable media?
			SFGAO_COMPRESSED = 0x04000000,         // Object is compressed (use alt color)
			SFGAO_BROWSABLE = 0x08000000,         // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
			SFGAO_NONENUMERATED = 0x00100000,         // is a non-enumerated object
			SFGAO_NEWCONTENT = 0x00200000,         // should show bold in explorer tree
			SFGAO_CANMONIKER = 0x00400000,         // defunct
			SFGAO_HASSTORAGE = 0x00400000,         // defunct
			SFGAO_STREAM = 0x00400000,         // supports BindToObject(IID_IStream)
			SFGAO_STORAGEANCESTOR = 0x00800000,         // may contain children with SFGAO_STORAGE or SFGAO_STREAM
			SFGAO_STORAGECAPMASK = 0x70C50008,         // for determining storage capabilities, ie for open/save semantics
		}

		public enum SICHINTF : uint
		{
			SICHINT_DISPLAY = 0x00000000,
			SICHINT_CANONICAL = 0x10000000,
			SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
			SICHINT_ALLFIELDS = 0x80000000
		}

		public enum GETPROPERTYSTOREFLAGS
		{
			// If no flags are specified (GPS_DEFAULT), a read-only property store is returned that includes properties for the file or item.
			// In the case that the shell item is a file, the property store contains:
			//     1. properties about the file from the file system
			//     2. properties from the file itself provided by the file's property handler, unless that file is offline,
			//     see GPS_OPENSLOWITEM
			//     3. if requested by the file's property handler and supported by the file system, properties stored in the
			//     alternate property store.
			//
			// Non-file shell items should return a similar read-only store
			//
			// Specifying other GPS_ flags modifies the store that is returned
			GPS_DEFAULT = 0x00000000,
			GPS_HANDLERPROPERTIESONLY = 0x00000001,   // only include properties directly from the file's property handler
			GPS_READWRITE = 0x00000002,   // Writable stores will only include handler properties
			GPS_TEMPORARY = 0x00000004,   // A read/write store that only holds properties for the lifetime of the IShellItem object
			GPS_FASTPROPERTIESONLY = 0x00000008,   // do not include any properties from the file's property handler (because the file's property handler will hit the disk)
			GPS_OPENSLOWITEM = 0x00000010,   // include properties from a file's property handler, even if it means retrieving the file from offline storage.
			GPS_DELAYCREATION = 0x00000020,   // delay the creation of the file's property handler until those properties are read, written, or enumerated
			GPS_BESTEFFORT = 0x00000040,   // For readonly stores, succeed and return all available properties, even if one or more sources of properties fails. Not valid with GPS_READWRITE.
			GPS_NO_OPLOCK = 0x00000080,   // some data sources protect the read property store with an oplock, this disables that
			GPS_MASK_VALID = 0x000000FF,
		}

		public enum ESFGAO : uint
		{
			SFGAO_CANCOPY = 0x00000001,
			SFGAO_CANMOVE = 0x00000002,
			SFGAO_CANLINK = 0x00000004,
			SFGAO_LINK = 0x00010000,
			SFGAO_SHARE = 0x00020000,
			SFGAO_READONLY = 0x00040000,
			SFGAO_HIDDEN = 0x00080000,
			SFGAO_FOLDER = 0x20000000,
			SFGAO_FILESYSTEM = 0x40000000,
			SFGAO_HASSUBFOLDER = 0x80000000,
		}

		public enum ESHCONTF
		{
			SHCONTF_FOLDERS = 0x0020,
			SHCONTF_NONFOLDERS = 0x0040,
			SHCONTF_INCLUDEHIDDEN = 0x0080,
			SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
			SHCONTF_NETPRINTERSRCH = 0x0200,
			SHCONTF_SHAREABLE = 0x0400,
			SHCONTF_STORAGE = 0x0800
		}

		public enum ESHGDN
		{
			SHGDN_NORMAL = 0x0000,
			SHGDN_INFOLDER = 0x0001,
			SHGDN_FOREDITING = 0x1000,
			SHGDN_FORADDRESSBAR = 0x4000,
			SHGDN_FORPARSING = 0x8000,
		}

		#endregion

		#region Interface

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
		public interface IShellItem
		{
			/// <summary>
			/// Binds to a handler for an item as specified by the handler ID value (BHID).
			/// </summary>
			/// <param name="pbc">A pointer to an IBindCtx  interface on a bind context object. Used to pass optional parameters to the handler. The contents of the bind context are handler-specific. For example, when binding to BHID_Stream, the STGM flags in the bind context indicate the mode of access desired (read versus read/write).</param>
			/// <param name="bhid">Reference to a GUID that specifies which handler will be created.</param>
			/// <param name="riid">IID of the object type to retrieve.</param>
			/// <param name="ppv">When this method returns, contains a pointer of type riid that is returned by the handler specified by rbhid.</param>
			void BindToHandler(IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

			/// <summary>
			/// Gets the parent of an IShellItem object.
			/// </summary>
			/// <param name="ppsi">The address of a pointer to the parent of an IShellItem interface.</param>
			void GetParent(out IShellItem ppsi);

			/// <summary>
			/// Gets the display name of the IShellItem object.
			/// </summary>
			/// <param name="sigdnName">One of the SIGDN values that indicates how the name should look.</param>
			/// <param name="ppszName">A value that, when this function returns successfully, receives the address of a pointer to the retrieved display name.</param>
			void GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

			/// <summary>
			/// Gets a requested set of attributes of the IShellItem object.
			/// </summary>
			/// <param name="sfgaoMask">Specifies the attributes to retrieve. One or more of the SFGAO values. Use a bitwise OR operator to determine the attributes to retrieve.</param>
			/// <param name="psfgaoAttribs">A pointer to a value that, when this method returns successfully, contains the requested attributes. One or more of the SFGAO values. Only those attributes specified by sfgaoMask are returned; other attribute values are undefined.</param>
			void GetAttributes(SFGAO sfgaoMask, out SFGAO psfgaoAttribs);

			/// <summary>
			/// Compares two IShellItem objects.
			/// </summary>
			/// <param name="psi">A pointer to an IShellItem object to compare with the existing IShellItem object. </param>
			/// <param name="hint">One of the SICHINTF values that determines how to perform the comparison. See SICHINTF for the list of possible values for this parameter.</param>
			/// <param name="piOrder">This parameter receives the result of the comparison. If the two items are the same this parameter equals zero; if they are different the parameter is nonzero.</param>
			void Compare(IShellItem psi, SICHINTF hint, out int piOrder);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93")]
		public interface IShellItem2 : IShellItem
		{
			// Not supported: IBindCtx.
			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int BindToHandler([In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetAttributes([In] SFGAO sfgaoMask, out SFGAO psfgaoAttribs);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);

			//out object ppv
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
			int GetPropertyStore([In] GETPROPERTYSTOREFLAGS Flags, [In] ref Guid riid, out IPropertyStore ppv);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetPropertyStoreWithCreateObject([In] GETPROPERTYSTOREFLAGS Flags, [In, MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid, out IntPtr ppv);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetPropertyStoreForKeys([In] ref PropertyKey rgKeys, [In] uint cKeys, [In] GETPROPERTYSTOREFLAGS Flags, [In] ref Guid riid, out IntPtr ppv);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, out IntPtr ppv);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Update(IntPtr pbc);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetProperty([In] ref PropertyKey key, out PropVariant ppropvar);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetCLSID([In] ref PropertyKey key, out Guid pclsid);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetFileTime([In] ref PropertyKey key, out System.Runtime.InteropServices.ComTypes.FILETIME pft);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetInt32([In] ref PropertyKey key, out int pi);

			[PreserveSig]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int GetString([In] ref PropertyKey key, [MarshalAs(UnmanagedType.LPWStr)] out string ppsz);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetUInt32([In] ref PropertyKey key, out uint pui);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetUInt64([In] ref PropertyKey key, out ulong pull);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetBool([In] ref PropertyKey key, out int pf);
		}

		/// <summary>
		///  managed equivalent of IShellFolder interface
		///  Pinvoke.net / Mod by Arik Poznanski - pooya parsa
		///  Msdn:      http://msdn.microsoft.com/en-us/library/windows/desktop/bb775075(v=vs.85).aspx
		///  Pinvoke:   http://pinvoke.net/default.aspx/Interfaces/IShellFolder.html
		/// </summary>
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214E6-0000-0000-C000-000000000046")]
		public interface IShellFolder
		{
			/// <summary>
			/// Translates a file object's or folder's display name into an item identifier list.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">Optional window handle</param>
			/// <param name="pbc">Optional bind context that controls the parsing operation. This parameter is normally set to NULL. </param>
			/// <param name="pszDisplayName">Null-terminated UNICODE string with the display name</param>
			/// <param name="pchEaten">Pointer to a ULONG value that receives the number of characters of the display name that was parsed.</param>
			/// <param name="ppidl"> Pointer to an ITEMIDLIST pointer that receives the item identifier list for the object.</param>
			/// <param name="pdwAttributes">Optional parameter that can be used to query for file attributes.this can be values from the SFGAO enum</param>
			void ParseDisplayName(IntPtr hwnd, IntPtr pbc, String pszDisplayName, UInt32 pchEaten, out IntPtr ppidl, UInt32 pdwAttributes);

			/// <summary>
			///Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface. 
			///Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.</param>
			/// <param name="grfFlags">Flags indicating which items to include in the  enumeration. For a list of possible values, see the SHCONTF enum. </param>
			/// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method. </param>
			void EnumObjects(IntPtr hwnd, ESHCONTF grfFlags, out IntPtr ppenumIDList);

			/// <summary>
			///Retrieves an IShellFolder object for a subfolder.
			// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
			/// <param name="riid">Identifier of the interface to return. </param>
			/// <param name="ppv">Address that receives the interface pointer.</param>
			void BindToObject(IntPtr pidl, IntPtr pbc, [In]ref Guid riid, out IntPtr ppv);

			/// <summary>
			/// Requests a pointer to an object's storage interface. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder. </param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be  used during this operation.</param>
			/// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
			/// <param name="ppv"> Address that receives the interface pointer specified by riid.</param>
			void BindToStorage(IntPtr pidl, IntPtr pbc, [In]ref Guid riid, out IntPtr ppv);

			/// <summary>
			/// Determines the relative order of two file objects or folders, given 
			/// their item identifier lists. Return value: If this method is 
			/// successful, the CODE field of the HRESULT contains one of the 
			/// following values (the code can be retrived using the helper function
			/// GetHResultCode): Negative A negative return value indicates that the first item should precede the second (pidl1 < pidl2). 
			//// 
			///Positive A positive return value indicates that the first item should
			///follow the second (pidl1 > pidl2).  Zero A return value of zero
			///indicates that the two items are the same (pidl1 = pidl2). 
			/// </summary>
			/// <param name="lParam">Value that specifies how the comparison  should be performed. The lower Sixteen bits of lParam define the sorting  rule. 
			///  The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from  the SHCIDS enum
			/// </param>
			/// <param name="pidl1">Pointer to the first item's ITEMIDLIST structure.</param>
			/// <param name="pidl2"> Pointer to the second item's ITEMIDLIST structure.</param>
			/// <returns></returns>
			[PreserveSig]
			Int32 CompareIDs(Int32 lParam, IntPtr pidl1, IntPtr pidl2);

			/// <summary>
			/// Requests an object that can be used to obtain information from or interact
			/// with a folder object.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window.</param>
			/// <param name="riid">Identifier of the requested interface.</param>
			/// <param name="ppv">Address of a pointer to the requested interface. </param>
			void CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

			/// <summary>
			/// Retrieves the attributes of one or more file objects or subfolders. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="cidl">Number of file objects from which to retrieve attributes. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which  uniquely identifies a file object relative to the parent folder.</param>
			/// <param name="rgfInOut">Address of a single ULONG value that, on entry contains the attributes that the caller is 
			/// requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum
			/// </param>
			void GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]IntPtr[] apidl, ref ESFGAO rgfInOut);

			/// <summary>
			/// Retrieves an OLE interface that can be used to carry out actions on the 
			/// specified file objects or folders. Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window that the client should specify if it displays a dialog box or message box.</param>
			/// <param name="cidl">Number of file objects or subfolders specified in the apidl parameter. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST  structures, each of which  uniquely identifies a file object or subfolder relative to the parent folder.</param>
			/// <param name="riid">Identifier of the COM interface object to return.</param>
			/// <param name="rgfReserved"> Reserved. </param>
			/// <param name="ppv">Pointer to the requested interface.</param>
			void GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]IntPtr[] apidl, [In] ref Guid riid, UInt32 rgfReserved, out IntPtr ppv);

			/// <summary>
			/// Retrieves the display name for the specified file object or subfolder. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL)  that uniquely identifies the file  object or subfolder relative to the parent  folder. </param>
			/// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values. </param>
			/// <param name="pName"> Address of a STRRET structure in which to return the display name.</param>
			void GetDisplayNameOf(IntPtr pidl, ESHGDN uFlags, out STRRET pName);

			/// <summary>
			/// Sets the display name of a file object or subfolder, changing the item
			/// identifier in the process.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd"> Handle to the owner window of any dialog or message boxes that the client displays.</param>
			/// <param name="pidl"> Pointer to an ITEMIDLIST structure that uniquely identifies the file object or subfolder relative to the parent folder. </param>
			/// <param name="pszName"> Pointer to a null-terminated string that specifies the new display name.</param>
			/// <param name="uFlags">Flags indicating the type of name specified by  the lpszName parameter. For a list of possible values, see the description of the SHGNO enum.</param>
			/// <param name="ppidlOut"></param>
			void SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, ESHCONTF uFlags, out IntPtr ppidlOut);

		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
		public interface IPropertyStore
		{
			uint GetCount([Out] out uint cProps);
			uint GetAt([In] uint iProp, out PropertyKey pkey);
			//uint GetValue([In] ref PropertyKey key, [Out] object pv);
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void GetValue([In] ref PropertyKey key, out object pv);

			uint SetValue([In] ref PropertyKey key, [In] PropVariant pv);
			uint Commit();
		}

		// IShellLink Interface
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214F9-0000-0000-C000-000000000046")]
		public interface IShellLinkW
		{
			uint GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
						 int cchMaxPath, ref WIN32_FIND_DATAW pfd, uint fFlags);
			uint GetIDList(out IntPtr ppidl);
			uint SetIDList(IntPtr pidl);
			uint GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName,
								int cchMaxName);
			uint SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
			uint GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
									 int cchMaxPath);
			uint SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
			uint GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
							  int cchMaxPath);
			uint SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
			uint GetHotKey(out ushort pwHotkey);
			uint SetHotKey(ushort wHotKey);
			uint GetShowCmd(out int piShowCmd);
			uint SetShowCmd(int iShowCmd);
			uint GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
								 int cchIconPath, out int piIcon);
			uint SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
			uint SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
								 uint dwReserved);
			uint Resolve(IntPtr hwnd, uint fFlags);
			uint SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

		// ShellLink CoClass (ShellLink object)
		[ComImport]
		[ClassInterface(ClassInterfaceType.None)]
		[Guid("00021401-0000-0000-C000-000000000046")]
		public class CShellLink { }

		#endregion

		#region Win32Apis
		
		public static class APIs
		{
			#region Const
			
			public const int S_OK = 0x00000000;
			public const int FACILITY_WIN32 = unchecked((int)0x80070000);
			public const int ERROR_NOT_FOUND = 1168;
			public const int MAX_PATH = 260;
			public const int INFOTIPSIZE = 1024;

			#endregion

			[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
			public static extern void SHCreateItemFromParsingName(
				[In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
				[In] IntPtr pbc,
				[In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
				[Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem2 ppv);

			[DllImport("Shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
			public static extern int HashData(
			 [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)] byte[] pbData,
			int cbData,
			 [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] piet,
			int outputLen);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

			public static int HRESULT_FROM_WIN32(int hResult)
			{
				return hResult <= 0 ? hResult : ((hResult & 0x0000FFFF) | FACILITY_WIN32);
			}

		}

		#endregion

		#region GUIDs

		public static class PropertyKeys
		{
			public static PropertyKey PKEY_Link_Comment = new PropertyKey(new Guid("b9b4b3fc-2b51-4a42-b5d8-324146afcf25"), 5);
			public static PropertyKey PKEY_Link_DateVisited = new PropertyKey(new Guid("5cbf2787-48cf-4208-b90e-ee5e5d420294"), 23);
			public static PropertyKey PKEY_Link_Description = new PropertyKey(new Guid("5cbf2787-48cf-4208-b90e-ee5e5d420294"), 21);
			public static PropertyKey PKEY_Link_Status = new PropertyKey(new Guid("b9b4b3fc-2b51-4a42-b5d8-324146afcf25"), 3);
			public static PropertyKey PKEY_Link_TargetExtension = new PropertyKey(new Guid("7a7d76f4-b630-4bd7-95ff-37cc51a975c9"), 2);
			public static PropertyKey PKEY_Link_TargetParsingPath = new PropertyKey(new Guid("b9b4b3fc-2b51-4a42-b5d8-324146afcf25"), 2);
			public static PropertyKey PKEY_Link_TargetSFGAOFlags = new PropertyKey(new Guid("b9b4b3fc-2b51-4a42-b5d8-324146afcf25"), 8);
			public static PropertyKey PKEY_Link_Arguments = new PropertyKey(new Guid("436f2667-14e2-4feb-b30a-146c53b5b674"), 100);
			public static PropertyKey PKEY_WinX_Hash = new PropertyKey(new Guid("fb8d2d7b-90d1-4e34-bf60-6eac09922bbf"), 2);
		}

		#endregion
	}
}
