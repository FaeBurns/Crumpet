using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

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
        InstanceReference instance = new BuiltinTypeInfo<string>().CreateInstance( "test");
        Assert.That(instance.Value, Is.EqualTo("test"));
    }

    [Test]
    public void TestObject_Constructs_Simple()
    {
        UserObjectTypeInfo testType = new UserObjectTypeInfo("testType", [new FieldInfo("testVar", new BuiltinTypeInfo<string>()), new FieldInfo("testVar2", new BuiltinTypeInfo<int>())]);
        InstanceReference instance = testType.CreateInstance();
        
        Assert.That(instance.Value, Is.TypeOf<UserObjectInstance>());
        Assert.That((instance.Value as UserObjectInstance)!.Fields["testVar"].Instance.Value, Is.EqualTo(""));
        Assert.That((instance.Value as UserObjectInstance)!.Fields["testVar2"].Instance.Value, Is.EqualTo(0));
        (instance.Value as UserObjectInstance)!.Fields["testVar2"].Instance.Value = 1;
        Assert.That((instance.Value as UserObjectInstance)!.Fields["testVar2"].Instance.Value, Is.EqualTo(1));
    }
    
    [Test]
    public void TestObject_Constructs_Layered()
    {
        UserObjectTypeInfo typeC = new UserObjectTypeInfo("typeC", new FieldInfo("fieldC", new BuiltinTypeInfo<string>()));
        UserObjectTypeInfo typeB = new UserObjectTypeInfo("typeB", new FieldInfo("fieldB", typeC));
        UserObjectTypeInfo typeA = new UserObjectTypeInfo("typeA", new FieldInfo("fieldA", typeB));

        InstanceReference instance = typeA.CreateInstance();
        
        Assert.That(instance.Value, Is.TypeOf<UserObjectInstance>());
        
        UserObjectInstance instanceA = (UserObjectInstance)instance.Value;
        Assert.That(instanceA.Fields["fieldA"].Type, Is.EqualTo(typeB));
        Assert.That(instanceA.Fields["fieldA"].Instance.Value, Is.TypeOf<UserObjectInstance>());
        
        UserObjectInstance instanceB = (UserObjectInstance)instanceA.Fields["fieldA"].Instance.Value!;
        Assert.That(instanceB.Fields["fieldB"].Type, Is.EqualTo(typeC));
        Assert.That(instanceB.Fields["fieldB"].Instance.Value, Is.TypeOf<UserObjectInstance>());
        
        UserObjectInstance instanceC = (UserObjectInstance)instanceB.Fields["fieldB"].Instance.Value!;
        Assert.That(instanceC.Fields["fieldC"].Type, Is.EqualTo(new BuiltinTypeInfo<string>()));
        Assert.That(instanceC.Fields["fieldC"].Instance.Value, Is.EqualTo(""));
    }

    [Test]
    public void TestObject_ValueSearcher_FindSingle()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new UserObjectTypeInfo("testType", new FieldInfo("testVar", new BuiltinTypeInfo<string>()), new FieldInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
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
        scope.Create(new VariableInfo("testObject", new UserObjectTypeInfo("testType", new FieldInfo("testVar", new BuiltinTypeInfo<string>()), new FieldInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
        ValueSearcher valueSearcher = new ValueSearcher(scope);
        
        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(2));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.Value, Is.EqualTo(""));
    }
    
    [Test]
    public void TestObject_ValueSearcher_FindTripple()
    {
        Scope scope = new Scope(null);
        scope.Create(new VariableInfo("testObject", new UserObjectTypeInfo("testType", new FieldInfo("testVar", new UserObjectTypeInfo("testType2", new FieldInfo("testVar2", new BuiltinTypeInfo<string>()))), new FieldInfo("testVar2", new BuiltinTypeInfo<int>()))));
        
        ValueSearcher valueSearcher = new ValueSearcher(scope);
        
        ValueSearchResult testVarSearchResult = valueSearcher.Find("testObject.testVar.testVar2");
        Assert.That(testVarSearchResult.DepthReached, Is.EqualTo(3));
        Assert.That(testVarSearchResult.Result, Is.Not.Null);
        Assert.That(testVarSearchResult.Result.Value, Is.EqualTo(""));
    }

    [Test]
    public void CloneValueTypes_Int()
    {
        // setup initial value
        InstanceReference initial = new BuiltinTypeInfo<int>().CreateInstance();
        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;
        
        // create copy
        InstanceReference copy = initial.GetForModifier(VariableModifier.COPY);
        Assert.That(copy.Value, Is.EqualTo(initial.Value));
        
        // change value in one
        copy.Value = 20;
        Assert.That(initial.Value, Is.EqualTo(10));
        Assert.That(copy.Value, Is.EqualTo(20));
    }
    
    [Test]
    public void CloneValueTypes_String()
    {
        // setup initial value
        InstanceReference initial = new BuiltinTypeInfo<string>().CreateInstance();
        Assert.That(initial.Value, Is.EqualTo(String.Empty));
        initial.Value = "test_1";
        
        // create copy
        InstanceReference copy = initial.GetForModifier(VariableModifier.COPY);
        Assert.That(copy.Value, Is.EqualTo(initial.Value));
        
        // change value in one
        copy.Value = "test_2";
        Assert.That(initial.Value, Is.EqualTo("test_1"));
        Assert.That(copy.Value, Is.EqualTo("test_2"));
    }

    [Test]
    public void ConvertType_IntToFloat()
    {
        // TODO implement
    }
}