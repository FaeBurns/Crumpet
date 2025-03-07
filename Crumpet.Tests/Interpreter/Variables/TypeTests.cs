using Crumpet.Interpreter;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;





namespace Crumpet.Tests.Interpreter.Variables;

[TestFixture]
public class TypeTests
{
    [Test]
    public void TestPrimitiveString_Default_Value()
    {
        Variable variable = new BuiltinTypeInfo<string>().CreateVariable();
        Assert.That(variable.Value, Is.EqualTo(""));
    }

    [Test]
    public void TestPrimitiveInt_Default_Value()
    {
        Variable variable = new BuiltinTypeInfo<int>().CreateVariable();
        Assert.That(variable.Value, Is.EqualTo(0));
    }

    [Test]
    public void TestPrimitiveFloat_Default_Value()
    {
        Variable variable = new BuiltinTypeInfo<float>().CreateVariable();
        Assert.That(variable.Value, Is.EqualTo(0.0f));
    }

    [Test]
    public void TestPrimitiveBool_Default_Value()
    {
        Variable variable = new BuiltinTypeInfo<bool>().CreateVariable();
        Assert.That(variable.Value, Is.EqualTo(false));
    }

    [Test]
    public void TestPrimitive_Sets_Value()
    {
        Variable variable = new BuiltinTypeInfo<string>().CreateVariable();
        Assert.That(variable.Value, Is.EqualTo(String.Empty));
        variable.Value = "test";
        Assert.That(variable.Value, Is.EqualTo("test"));
    }

    [Test]
    public void TestObject_Constructs_Simple()
    {
        UserObjectTypeInfo testType = new UserObjectTypeInfo("testType", [new FieldInfo("testVar", new BuiltinTypeInfo<string>()), new FieldInfo("testVar2", new BuiltinTypeInfo<int>())]);
        Variable variable = testType.CreateVariable();

        Assert.That(variable.Value, Is.TypeOf<UserObjectInstance>());
        Assert.That((variable.Value as UserObjectInstance)!.Fields["testVar"].Value, Is.EqualTo(""));
        Assert.That((variable.Value as UserObjectInstance)!.Fields["testVar2"].Value, Is.EqualTo(0));
        (variable.Value as UserObjectInstance)!.Fields["testVar2"].Value = 1;
        Assert.That((variable.Value as UserObjectInstance)!.Fields["testVar2"].Value, Is.EqualTo(1));
    }

    [Test]
    public void TestObject_Constructs_Layered()
    {
        UserObjectTypeInfo typeC = new UserObjectTypeInfo("typeC", new FieldInfo("fieldC", new BuiltinTypeInfo<string>()));
        UserObjectTypeInfo typeB = new UserObjectTypeInfo("typeB", new FieldInfo("fieldB", typeC));
        UserObjectTypeInfo typeA = new UserObjectTypeInfo("typeA", new FieldInfo("fieldA", typeB));

        Variable variable = typeA.CreateVariable();

        Assert.That(variable.Value, Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableA = (UserObjectInstance)variable.Value;
        Assert.That(variableA.Fields["fieldA"].Type, Is.EqualTo(typeB));
        Assert.That(variableA.Fields["fieldA"].Value, Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableB = (UserObjectInstance)variableA.Fields["fieldA"].Value!;
        Assert.That(variableB.Fields["fieldB"].Type, Is.EqualTo(typeC));
        Assert.That(variableB.Fields["fieldB"].Value, Is.TypeOf<UserObjectInstance>());

        UserObjectInstance variableC = (UserObjectInstance)variableB.Fields["fieldB"].Value!;
        Assert.That(variableC.Fields["fieldC"].Type, Is.EqualTo(new BuiltinTypeInfo<string>()));
        Assert.That(variableC.Fields["fieldC"].Value, Is.EqualTo(""));
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
        Variable initial = new BuiltinTypeInfo<int>().CreateVariable();
        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;

        // create copy
        Variable copy = Variable.CreateCopy(initial);
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
        Variable initial = new BuiltinTypeInfo<string>().CreateVariable();
        Assert.That(initial.Value, Is.EqualTo(String.Empty));
        initial.Value = "test_1";

        // create copy
        Variable copy = Variable.CreateCopy(initial);
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