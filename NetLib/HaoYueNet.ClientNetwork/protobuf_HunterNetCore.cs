//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: proto/protobuf_HunterNetCore.proto
namespace HunterProtobufCore
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HunterNet_C2S")]
  public partial class HunterNet_C2S : global::ProtoBuf.IExtensible
  {
    public HunterNet_C2S() {}
    
    private int? _HunterNetCore_CmdID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"HunterNetCore_CmdID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int? HunterNetCore_CmdID
    {
      get { return _HunterNetCore_CmdID; }
      set { _HunterNetCore_CmdID = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool HunterNetCore_CmdIDSpecified
    {
      get { return this._HunterNetCore_CmdID != null; }
      set { if (value == (this._HunterNetCore_CmdID== null)) this._HunterNetCore_CmdID = value ? this.HunterNetCore_CmdID : (int?)null; }
    }
    private bool ShouldSerializeHunterNetCore_CmdID() { return HunterNetCore_CmdIDSpecified; }
    private void ResetHunterNetCore_CmdID() { HunterNetCore_CmdIDSpecified = false; }
    
    private byte[] _HunterNetCore_Data;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"HunterNetCore_Data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] HunterNetCore_Data
    {
      get { return _HunterNetCore_Data; }
      set { _HunterNetCore_Data = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool HunterNetCore_DataSpecified
    {
      get { return this._HunterNetCore_Data != null; }
      set { if (value == (this._HunterNetCore_Data== null)) this._HunterNetCore_Data = value ? this.HunterNetCore_Data : (byte[])null; }
    }
    private bool ShouldSerializeHunterNetCore_Data() { return HunterNetCore_DataSpecified; }
    private void ResetHunterNetCore_Data() { HunterNetCore_DataSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HunterNet_S2C")]
  public partial class HunterNet_S2C : global::ProtoBuf.IExtensible
  {
    public HunterNet_S2C() {}
    
    private int? _HunterNetCore_CmdID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"HunterNetCore_CmdID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int? HunterNetCore_CmdID
    {
      get { return _HunterNetCore_CmdID; }
      set { _HunterNetCore_CmdID = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool HunterNetCore_CmdIDSpecified
    {
      get { return this._HunterNetCore_CmdID != null; }
      set { if (value == (this._HunterNetCore_CmdID== null)) this._HunterNetCore_CmdID = value ? this.HunterNetCore_CmdID : (int?)null; }
    }
    private bool ShouldSerializeHunterNetCore_CmdID() { return HunterNetCore_CmdIDSpecified; }
    private void ResetHunterNetCore_CmdID() { HunterNetCore_CmdIDSpecified = false; }
    
    private int? _HunterNetCore_ERRORCode;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"HunterNetCore_ERRORCode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int? HunterNetCore_ERRORCode
    {
      get { return _HunterNetCore_ERRORCode; }
      set { _HunterNetCore_ERRORCode = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool HunterNetCore_ERRORCodeSpecified
    {
      get { return this._HunterNetCore_ERRORCode != null; }
      set { if (value == (this._HunterNetCore_ERRORCode== null)) this._HunterNetCore_ERRORCode = value ? this.HunterNetCore_ERRORCode : (int?)null; }
    }
    private bool ShouldSerializeHunterNetCore_ERRORCode() { return HunterNetCore_ERRORCodeSpecified; }
    private void ResetHunterNetCore_ERRORCode() { HunterNetCore_ERRORCodeSpecified = false; }
    
    private byte[] _HunterNetCore_Data;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"HunterNetCore_Data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] HunterNetCore_Data
    {
      get { return _HunterNetCore_Data; }
      set { _HunterNetCore_Data = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool HunterNetCore_DataSpecified
    {
      get { return this._HunterNetCore_Data != null; }
      set { if (value == (this._HunterNetCore_Data== null)) this._HunterNetCore_Data = value ? this.HunterNetCore_Data : (byte[])null; }
    }
    private bool ShouldSerializeHunterNetCore_Data() { return HunterNetCore_DataSpecified; }
    private void ResetHunterNetCore_Data() { HunterNetCore_DataSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}