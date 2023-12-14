// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protobuf_HunterNetCore.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbr = global::Google.Protobuf.Reflection;
namespace HunterProtobufCore
{

    /// <summary>Holder for reflection information generated from protobuf_HunterNetCore.proto</summary>
    public static partial class ProtobufHunterNetCoreReflection {

    #region Descriptor
    /// <summary>File descriptor for protobuf_HunterNetCore.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtobufHunterNetCoreReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Chxwcm90b2J1Zl9IdW50ZXJOZXRDb3JlLnByb3RvEhJIdW50ZXJQcm90b2J1",
            "ZkNvcmUiSAoNSHVudGVyTmV0X0MyUxIbChNIdW50ZXJOZXRDb3JlX0NtZElE",
            "GAEgASgFEhoKEkh1bnRlck5ldENvcmVfRGF0YRgCIAEoDCJpCg1IdW50ZXJO",
            "ZXRfUzJDEhsKE0h1bnRlck5ldENvcmVfQ21kSUQYASABKAUSHwoXSHVudGVy",
            "TmV0Q29yZV9FUlJPUkNvZGUYAiABKAUSGgoSSHVudGVyTmV0Q29yZV9EYXRh",
            "GAMgASgMQgJIAWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::HunterProtobufCore.HunterNet_C2S), global::HunterProtobufCore.HunterNet_C2S.Parser, new[]{ "HunterNetCoreCmdID", "HunterNetCoreData" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::HunterProtobufCore.HunterNet_S2C), global::HunterProtobufCore.HunterNet_S2C.Parser, new[]{ "HunterNetCoreCmdID", "HunterNetCoreERRORCode", "HunterNetCoreData" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///上行
  /// </summary>
  public sealed partial class HunterNet_C2S : pb::IMessage<HunterNet_C2S>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HunterNet_C2S> _parser = new pb::MessageParser<HunterNet_C2S>(() => new HunterNet_C2S());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HunterNet_C2S> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HunterProtobufCore.ProtobufHunterNetCoreReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_C2S() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_C2S(HunterNet_C2S other) : this() {
      hunterNetCoreCmdID_ = other.hunterNetCoreCmdID_;
      hunterNetCoreData_ = other.hunterNetCoreData_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_C2S Clone() {
      return new HunterNet_C2S(this);
    }

    /// <summary>Field number for the "HunterNetCore_CmdID" field.</summary>
    public const int HunterNetCoreCmdIDFieldNumber = 1;
    private int hunterNetCoreCmdID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int HunterNetCoreCmdID {
      get { return hunterNetCoreCmdID_; }
      set {
        hunterNetCoreCmdID_ = value;
      }
    }

    /// <summary>Field number for the "HunterNetCore_Data" field.</summary>
    public const int HunterNetCoreDataFieldNumber = 2;
    private pb::ByteString hunterNetCoreData_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString HunterNetCoreData {
      get { return hunterNetCoreData_; }
      set {
        hunterNetCoreData_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HunterNet_C2S);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HunterNet_C2S other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (HunterNetCoreCmdID != other.HunterNetCoreCmdID) return false;
      if (HunterNetCoreData != other.HunterNetCoreData) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HunterNetCoreCmdID != 0) hash ^= HunterNetCoreCmdID.GetHashCode();
      if (HunterNetCoreData.Length != 0) hash ^= HunterNetCoreData.GetHashCode();
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
      if (HunterNetCoreCmdID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(HunterNetCoreCmdID);
      }
      if (HunterNetCoreData.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HunterNetCoreCmdID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(HunterNetCoreCmdID);
      }
      if (HunterNetCoreData.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HunterNetCoreCmdID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(HunterNetCoreCmdID);
      }
      if (HunterNetCoreData.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HunterNet_C2S other) {
      if (other == null) {
        return;
      }
      if (other.HunterNetCoreCmdID != 0) {
        HunterNetCoreCmdID = other.HunterNetCoreCmdID;
      }
      if (other.HunterNetCoreData.Length != 0) {
        HunterNetCoreData = other.HunterNetCoreData;
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
            HunterNetCoreCmdID = input.ReadInt32();
            break;
          }
          case 18: {
            HunterNetCoreData = input.ReadBytes();
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
            HunterNetCoreCmdID = input.ReadInt32();
            break;
          }
          case 18: {
            HunterNetCoreData = input.ReadBytes();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  ///下行
  /// </summary>
  public sealed partial class HunterNet_S2C : pb::IMessage<HunterNet_S2C>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HunterNet_S2C> _parser = new pb::MessageParser<HunterNet_S2C>(() => new HunterNet_S2C());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HunterNet_S2C> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HunterProtobufCore.ProtobufHunterNetCoreReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_S2C() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_S2C(HunterNet_S2C other) : this() {
      hunterNetCoreCmdID_ = other.hunterNetCoreCmdID_;
      hunterNetCoreERRORCode_ = other.hunterNetCoreERRORCode_;
      hunterNetCoreData_ = other.hunterNetCoreData_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HunterNet_S2C Clone() {
      return new HunterNet_S2C(this);
    }

    /// <summary>Field number for the "HunterNetCore_CmdID" field.</summary>
    public const int HunterNetCoreCmdIDFieldNumber = 1;
    private int hunterNetCoreCmdID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int HunterNetCoreCmdID {
      get { return hunterNetCoreCmdID_; }
      set {
        hunterNetCoreCmdID_ = value;
      }
    }

    /// <summary>Field number for the "HunterNetCore_ERRORCode" field.</summary>
    public const int HunterNetCoreERRORCodeFieldNumber = 2;
    private int hunterNetCoreERRORCode_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int HunterNetCoreERRORCode {
      get { return hunterNetCoreERRORCode_; }
      set {
        hunterNetCoreERRORCode_ = value;
      }
    }

    /// <summary>Field number for the "HunterNetCore_Data" field.</summary>
    public const int HunterNetCoreDataFieldNumber = 3;
    private pb::ByteString hunterNetCoreData_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString HunterNetCoreData {
      get { return hunterNetCoreData_; }
      set {
        hunterNetCoreData_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HunterNet_S2C);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HunterNet_S2C other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (HunterNetCoreCmdID != other.HunterNetCoreCmdID) return false;
      if (HunterNetCoreERRORCode != other.HunterNetCoreERRORCode) return false;
      if (HunterNetCoreData != other.HunterNetCoreData) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HunterNetCoreCmdID != 0) hash ^= HunterNetCoreCmdID.GetHashCode();
      if (HunterNetCoreERRORCode != 0) hash ^= HunterNetCoreERRORCode.GetHashCode();
      if (HunterNetCoreData.Length != 0) hash ^= HunterNetCoreData.GetHashCode();
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
      if (HunterNetCoreCmdID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(HunterNetCoreCmdID);
      }
      if (HunterNetCoreERRORCode != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(HunterNetCoreERRORCode);
      }
      if (HunterNetCoreData.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HunterNetCoreCmdID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(HunterNetCoreCmdID);
      }
      if (HunterNetCoreERRORCode != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(HunterNetCoreERRORCode);
      }
      if (HunterNetCoreData.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HunterNetCoreCmdID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(HunterNetCoreCmdID);
      }
      if (HunterNetCoreERRORCode != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(HunterNetCoreERRORCode);
      }
      if (HunterNetCoreData.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(HunterNetCoreData);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HunterNet_S2C other) {
      if (other == null) {
        return;
      }
      if (other.HunterNetCoreCmdID != 0) {
        HunterNetCoreCmdID = other.HunterNetCoreCmdID;
      }
      if (other.HunterNetCoreERRORCode != 0) {
        HunterNetCoreERRORCode = other.HunterNetCoreERRORCode;
      }
      if (other.HunterNetCoreData.Length != 0) {
        HunterNetCoreData = other.HunterNetCoreData;
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
            HunterNetCoreCmdID = input.ReadInt32();
            break;
          }
          case 16: {
            HunterNetCoreERRORCode = input.ReadInt32();
            break;
          }
          case 26: {
            HunterNetCoreData = input.ReadBytes();
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
            HunterNetCoreCmdID = input.ReadInt32();
            break;
          }
          case 16: {
            HunterNetCoreERRORCode = input.ReadInt32();
            break;
          }
          case 26: {
            HunterNetCoreData = input.ReadBytes();
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
