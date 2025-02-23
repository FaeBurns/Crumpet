using Crumpet.Interpreter.Interpreter;
using Crumpet.Interpreter.Interpreter.Variables;
using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Tests.Interpreter.Variables;

[TestFixture]
public class TypeTests
{
    [Test]
    public void TestPrimitiveString_Default_Value()
    {
        InstanceReference instance = new BuiltinTypeInfo<string>().CreateInstance();
        Assert.That(instance.Value, Is.EqualTo(""));
    }

    [Test]
    public void TestPrimitiveInt_Default_Value()
    {
        InstanceReference instance = new BuiltinTypeInfo<int>().CreateInstance();
        Assert.That(instance.Value, Is.EqualTo(0));
    }
    
    [Test]
    public void TestPrimitiveFloat_Default_Value()
    {
        InstanceReference instance = new BuiltinTypeInfo<float>().CreateInstance();
        Assert.That(instance.Value, Is.EqualTo(0.0f));
    }
    
    [Test]
    public void TestPrimitiveBool_Default_Value()
    {
        InstanceReference instance = new BuiltinTypeInfo<bool>().CreateInstance();
        Assert.That(instance.Value, Is.EqualTo(false));
    }
    
    [Test]
    public void TestPrimitive_Sets_Value()
    {
        InstanceReference instance = new BuiltinTypeInfo<string>().CreateInstance();
        Assert.That(instance.Value, Is.EqualTo(String.Empty));
        instance.Value = "test";
        Assert.That(instance.Value, Is.EqualTo("test"));
    }

    [Test]
    public void TestPrimitive_Constructor_Sets()
    {
        InstanceReference instance = InstanceReference.Create(new BuiltinTypeInfo<string>(), "test");
        Assert.That(instance.Value, Is.EqualTo("test"));
    }

    [Test]
    public void TestObject_Constructs_Simple()
    {
        ObjectTypeInfo testType = new ObjectTypeInfo("testType", [new VariableInfo("testVar", new BuiltinTypeInfo<string>()), new VariableInfo("testVar2", new BuiltinTypeInfo<int>())]);
        InstanceReference instance = testType.CreateInstance();
        
        Assert.That(instance.Value, Is.TypeOf<ObjectInstance>());
        Assert.That((instance.Value as ObjectInstance)!.Fields["testVar"].Value, Is.EqualTo(""));
        Assert.That((instance.Value as ObjectInstance)!.Fields["testVar2"].Value, Is.EqualTo(0));
        (instance.Value as ObjectInstance)!.Fields["testVar2"].Value = 1;
        Assert.That((instance.Value as ObjectInstance)!.Fields["testVar2"].Value, Is.EqualTo(1));
    }
    
    [Test]
    public void TestObject_Constructs_Layered()
    {
        ObjectTypeInfo typeC = new ObjectTypeInfo("typeC", new VariableInfo("fieldC", new BuiltinTypeInfo<string>()));
        ObjectTypeInfo typeB = new ObjectTypeInfo("typeB", new VariableInfo("fieldB", typeC));
        ObjectTypeInfo typeA = new ObjectTypeInfo("typeA", new VariableInfo("fieldA", typeB));

        InstanceReference instance = typeA.CreateInstance();
        
        Assert.That(instance.Value, Is.TypeOf<ObjectInstance>());
        
        ObjectInstance instanceA = (ObjectInstance)instance.Value!;
        Assert.That(instanceA.Fields["fieldA"].Type, Is.EqualTo(typeB));
        Assert.That(instanceA.Fields["fieldA"].Value, Is.TypeOf<ObjectInstance>());
        
        ObjectInstance instanceB = (ObjectInstance)instanceA.Fields["fieldA"].Value!;
        Assert.That(instanceB.Fields["fieldB"].Type, Is.EqualTo(typeC));
        Assert.That(instanceB.Fields["fieldB"].Value, Is.TypeOf<ObjectInstance>());
        
        ObjectInstance instanceC = (ObjectInstance)instanceB.Fields["fieldB"].Value!;
        Assert.That(instanceC.Fields["fieldC"].Type, Is.EqualTo(new BuiltinTypeInfo<string>()));
        Assert.That(instanceC.Fields["fieldC"].Value, Is.EqualTo(""));
    }

    [Test]
    public void TestObject_ValueSearcher_FindSingle()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new ObjectTypeInfo("testType", new VariableInfo("testVar", new BuiltinTypeInfo<string>()), new VariableInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
        ValueSearcher valueSearcher = new ValueSearcher(scope);
        ValueSearchResult testObjectSearchResult = valueSearcher.Find("testObject");
        
        Assert.That(testObjectSearchResult.DepthReached, Is.EqualTo(1)); // equal to input segments array size
        Assert.That(testObjectSearchResult.Result, Is.Not.Null);
        Assert.That(testObjectSearchResult.Result.Type.TypeName, Is.EqualTo("testType"));
    }
    
    [Test]
    public void TestObject_ValueSearcher_FindDouble()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new ObjectTypeInfo("testType", new VariableInfo("testVar", new BuiltinTypeInfo<string>()), new VariableInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
        ValueSearcher valueSearcher = new ValueSearcher(scope);
        ValueSearchResult testObjectSearchResult = valueSearcher.Find("testObject");
        
        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(2));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.Value, Is.EqualTo(""));
    }
    
    [Test]
    public void TestObject_ValueSearcher_FindTripple()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new ObjectTypeInfo("testType", new VariableInfo("testVar", new ObjectTypeInfo("testType2", new VariableInfo("testVar2", new BuiltinTypeInfo<string>()))), new VariableInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
        ValueSearcher valueSearcher = new ValueSearcher(scope);
        ValueSearchResult testObjectSearchResult = valueSearcher.Find("testObject");
        
        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar.testVar2");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(3));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.Value, Is.EqualTo(""));
    }
    
}