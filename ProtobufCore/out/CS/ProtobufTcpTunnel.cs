// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protobuf_TcpTunnel.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace AxibugProtobuf {

  /// <summary>Holder for reflection information generated from protobuf_TcpTunnel.proto</summary>
  public static partial class ProtobufTcpTunnelReflection {

    #region Descriptor
    /// <summary>File descriptor for protobuf_TcpTunnel.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtobufTcpTunnelReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Chhwcm90b2J1Zl9UY3BUdW5uZWwucHJvdG8SDkF4aWJ1Z1Byb3RvYnVmIj4K",
            "HFByb3RvYnVmX1RjcFR1bm5lbF9IZWxsVG9TZXYSCwoDVUlEGAEgASgDEhEK",
            "CXRhcmdldFVJRBgCIAEoAyI1CiFQcm90b2J1Zl9UY3BUdW5uZWxfSGVsbFRv",
            "U2V2X1JFU1ASEAoIVGFza0dVSUQYASABKAkiiQEKIFByb3RvYnVmX1RjcFR1",
            "bm5lbF9Cb3RoSW5mb19SRVNQEhAKCFRhc2tHVUlEGAEgASgJEhEKCXRhcmdl",
            "dFVJRBgCIAEoAxIMCgRteUlQGAMgASgJEg4KBm15UG9ydBgEIAEoBRIPCgdv",
            "dGhlcklQGAUgASgJEhEKCW90aGVyUG9ydBgGIAEoBUICSAFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::AxibugProtobuf.Protobuf_TcpTunnel_HellToSev), global::AxibugProtobuf.Protobuf_TcpTunnel_HellToSev.Parser, new[]{ "UID", "TargetUID" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::AxibugProtobuf.Protobuf_TcpTunnel_HellToSev_RESP), global::AxibugProtobuf.Protobuf_TcpTunnel_HellToSev_RESP.Parser, new[]{ "TaskGUID" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::AxibugProtobuf.Protobuf_TcpTunnel_BothInfo_RESP), global::AxibugProtobuf.Protobuf_TcpTunnel_BothInfo_RESP.Parser, new[]{ "TaskGUID", "TargetUID", "MyIP", "MyPort", "OtherIP", "OtherPort" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///Tcp打洞请求
  /// </summary>
  public sealed partial class Protobuf_TcpTunnel_HellToSev : pb::IMessage<Protobuf_TcpTunnel_HellToSev>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Protobuf_TcpTunnel_HellToSev> _parser = new pb::MessageParser<Protobuf_TcpTunnel_HellToSev>(() => new Protobuf_TcpTunnel_HellToSev());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Protobuf_TcpTunnel_HellToSev> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxibugProtobuf.ProtobufTcpTunnelReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev(Protobuf_TcpTunnel_HellToSev other) : this() {
      uID_ = other.uID_;
      targetUID_ = other.targetUID_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev Clone() {
      return new Protobuf_TcpTunnel_HellToSev(this);
    }

    /// <summary>Field number for the "UID" field.</summary>
    public const int UIDFieldNumber = 1;
    private long uID_;
    /// <summary>
    ///自己的UID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UID {
      get { return uID_; }
      set {
        uID_ = value;
      }
    }

    /// <summary>Field number for the "targetUID" field.</summary>
    public const int TargetUIDFieldNumber = 2;
    private long targetUID_;
    /// <summary>
    ///目标的UID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long TargetUID {
      get { return targetUID_; }
      set {
        targetUID_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Protobuf_TcpTunnel_HellToSev);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Protobuf_TcpTunnel_HellToSev other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UID != other.UID) return false;
      if (TargetUID != other.TargetUID) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UID != 0L) hash ^= UID.GetHashCode();
      if (TargetUID != 0L) hash ^= TargetUID.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (UID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UID);
      }
      if (TargetUID != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(TargetUID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (UID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UID);
      }
      if (TargetUID != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(TargetUID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UID);
      }
      if (TargetUID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(TargetUID);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Protobuf_TcpTunnel_HellToSev other) {
      if (other == null) {
        return;
      }
      if (other.UID != 0L) {
        UID = other.UID;
      }
      if (other.TargetUID != 0L) {
        TargetUID = other.TargetUID;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            UID = input.ReadInt64();
            break;
          }
          case 16: {
            TargetUID = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            UID = input.ReadInt64();
            break;
          }
          case 16: {
            TargetUID = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  ///Tcp打洞请求 下行
  /// </summary>
  public sealed partial class Protobuf_TcpTunnel_HellToSev_RESP : pb::IMessage<Protobuf_TcpTunnel_HellToSev_RESP>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Protobuf_TcpTunnel_HellToSev_RESP> _parser = new pb::MessageParser<Protobuf_TcpTunnel_HellToSev_RESP>(() => new Protobuf_TcpTunnel_HellToSev_RESP());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Protobuf_TcpTunnel_HellToSev_RESP> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxibugProtobuf.ProtobufTcpTunnelReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev_RESP() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev_RESP(Protobuf_TcpTunnel_HellToSev_RESP other) : this() {
      taskGUID_ = other.taskGUID_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_HellToSev_RESP Clone() {
      return new Protobuf_TcpTunnel_HellToSev_RESP(this);
    }

    /// <summary>Field number for the "TaskGUID" field.</summary>
    public const int TaskGUIDFieldNumber = 1;
    private string taskGUID_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TaskGUID {
      get { return taskGUID_; }
      set {
        taskGUID_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Protobuf_TcpTunnel_HellToSev_RESP);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Protobuf_TcpTunnel_HellToSev_RESP other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TaskGUID != other.TaskGUID) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TaskGUID.Length != 0) hash ^= TaskGUID.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (TaskGUID.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(TaskGUID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (TaskGUID.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(TaskGUID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TaskGUID.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TaskGUID);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Protobuf_TcpTunnel_HellToSev_RESP other) {
      if (other == null) {
        return;
      }
      if (other.TaskGUID.Length != 0) {
        TaskGUID = other.TaskGUID;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            TaskGUID = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            TaskGUID = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  ///双方信息 下行
  /// </summary>
  public sealed partial class Protobuf_TcpTunnel_BothInfo_RESP : pb::IMessage<Protobuf_TcpTunnel_BothInfo_RESP>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Protobuf_TcpTunnel_BothInfo_RESP> _parser = new pb::MessageParser<Protobuf_TcpTunnel_BothInfo_RESP>(() => new Protobuf_TcpTunnel_BothInfo_RESP());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Protobuf_TcpTunnel_BothInfo_RESP> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxibugProtobuf.ProtobufTcpTunnelReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_BothInfo_RESP() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_BothInfo_RESP(Protobuf_TcpTunnel_BothInfo_RESP other) : this() {
      taskGUID_ = other.taskGUID_;
      targetUID_ = other.targetUID_;
      myIP_ = other.myIP_;
      myPort_ = other.myPort_;
      otherIP_ = other.otherIP_;
      otherPort_ = other.otherPort_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Protobuf_TcpTunnel_BothInfo_RESP Clone() {
      return new Protobuf_TcpTunnel_BothInfo_RESP(this);
    }

    /// <summary>Field number for the "TaskGUID" field.</summary>
    public const int TaskGUIDFieldNumber = 1;
    private string taskGUID_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TaskGUID {
      get { return taskGUID_; }
      set {
        taskGUID_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "targetUID" field.</summary>
    public const int TargetUIDFieldNumber = 2;
    private long targetUID_;
    /// <summary>
    ///目标的UID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long TargetUID {
      get { return targetUID_; }
      set {
        targetUID_ = value;
      }
    }

    /// <summary>Field number for the "myIP" field.</summary>
    public const int MyIPFieldNumber = 3;
    private string myIP_ = "";
    /// <summary>
    ///自己的IP
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MyIP {
      get { return myIP_; }
      set {
        myIP_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "myPort" field.</summary>
    public const int MyPortFieldNumber = 4;
    private int myPort_;
    /// <summary>
    ///自己的Port
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MyPort {
      get { return myPort_; }
      set {
        myPort_ = value;
      }
    }

    /// <summary>Field number for the "otherIP" field.</summary>
    public const int OtherIPFieldNumber = 5;
    private string otherIP_ = "";
    /// <summary>
    ///对方的IP
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OtherIP {
      get { return otherIP_; }
      set {
        otherIP_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "otherPort" field.</summary>
    public const int OtherPortFieldNumber = 6;
    private int otherPort_;
    /// <summary>
    ///对方的Port
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int OtherPort {
      get { return otherPort_; }
      set {
        otherPort_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Protobuf_TcpTunnel_BothInfo_RESP);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Protobuf_TcpTunnel_BothInfo_RESP other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TaskGUID != other.TaskGUID) return false;
      if (TargetUID != other.TargetUID) return false;
      if (MyIP != other.MyIP) return false;
      if (MyPort != other.MyPort) return false;
      if (OtherIP != other.OtherIP) return false;
      if (OtherPort != other.OtherPort) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TaskGUID.Length != 0) hash ^= TaskGUID.GetHashCode();
      if (TargetUID != 0L) hash ^= TargetUID.GetHashCode();
      if (MyIP.Length != 0) hash ^= MyIP.GetHashCode();
      if (MyPort != 0) hash ^= MyPort.GetHashCode();
      if (OtherIP.Length != 0) hash ^= OtherIP.GetHashCode();
      if (OtherPort != 0) hash ^= OtherPort.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (TaskGUID.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(TaskGUID);
      }
      if (TargetUID != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(TargetUID);
      }
      if (MyIP.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(MyIP);
      }
      if (MyPort != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(MyPort);
      }
      if (OtherIP.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(OtherIP);
      }
      if (OtherPort != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(OtherPort);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (TaskGUID.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(TaskGUID);
      }
      if (TargetUID != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(TargetUID);
      }
      if (MyIP.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(MyIP);
      }
      if (MyPort != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(MyPort);
      }
      if (OtherIP.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(OtherIP);
      }
      if (OtherPort != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(OtherPort);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TaskGUID.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TaskGUID);
      }
      if (TargetUID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(TargetUID);
      }
      if (MyIP.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MyIP);
      }
      if (MyPort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MyPort);
      }
      if (OtherIP.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OtherIP);
      }
      if (OtherPort != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(OtherPort);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Protobuf_TcpTunnel_BothInfo_RESP other) {
      if (other == null) {
        return;
      }
      if (other.TaskGUID.Length != 0) {
        TaskGUID = other.TaskGUID;
      }
      if (other.TargetUID != 0L) {
        TargetUID = other.TargetUID;
      }
      if (other.MyIP.Length != 0) {
        MyIP = other.MyIP;
      }
      if (other.MyPort != 0) {
        MyPort = other.MyPort;
      }
      if (other.OtherIP.Length != 0) {
        OtherIP = other.OtherIP;
      }
      if (other.OtherPort != 0) {
        OtherPort = other.OtherPort;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            TaskGUID = input.ReadString();
            break;
          }
          case 16: {
            TargetUID = input.ReadInt64();
            break;
          }
          case 26: {
            MyIP = input.ReadString();
            break;
          }
          case 32: {
            MyPort = input.ReadInt32();
            break;
          }
          case 42: {
            OtherIP = input.ReadString();
            break;
          }
          case 48: {
            OtherPort = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            TaskGUID = input.ReadString();
            break;
          }
          case 16: {
            TargetUID = input.ReadInt64();
            break;
          }
          case 26: {
            MyIP = input.ReadString();
            break;
          }
          case 32: {
            MyPort = input.ReadInt32();
            break;
          }
          case 42: {
            OtherIP = input.ReadString();
            break;
          }
          case 48: {
            OtherPort = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
