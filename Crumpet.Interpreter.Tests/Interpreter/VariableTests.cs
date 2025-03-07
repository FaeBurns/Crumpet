using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Tests.Interpreter;

[TestFixture]
public class VariableTests
{
    [Test]
    public void PrimitiveTypes()
    {

    }

    [Test]
    public void VariableCollection_Create_Valid()
    {
        VariableCollection collection = new VariableCollection();

        collection.Create(new VariableInfo("testVar", new BuiltinTypeInfo<string>()));
        Assert.That(collection.Has("testVar"));
        Assert.That(collection.CheckType("testVar", new BuiltinTypeInfo<string>()));
        Assert.That(collection.GetVariable("testVar").Value, Is.EqualTo(String.Empty));
    }

    [Test]
    public void VariableCollection_Saves_Changes()
    {
        VariableCollection collection = new VariableCollection();
        collection.Create(new VariableInfo("testVar", new BuiltinTypeInfo<string>()));

        Assert.That(collection.GetVariable("testVar").Value, Is.EqualTo(String.Empty));
        collection.GetVariable("testVar").Value = "testValue";
        Assert.That(collection.GetVariable("testVar").Value, Is.EqualTo("testValue"));
    }

    [Test]
    public void AssignVariable_Copy()
    {
        // setup variables
        Variable initial = new BuiltinTypeInfo<int>().CreateVariable();
        Variable copy = new BuiltinTypeInfo<int>().CreateVariable();

        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;
        copy.Value = initial;

        // create copy
        Assert.That(copy.Value, Is.EqualTo(initial.Value));

        // change value in one and test it does not propagate to the other
        copy.Value = 20;
        Assert.That(initial.Value, Is.EqualTo(10));
        Assert.That(copy.Value, Is.EqualTo(20));
    }

    [Test]
    public void AssignVariable_Pointer()
    {
        // setup variables
        Variable initial = new BuiltinTypeInfo<int>().CreateVariable();
        Variable pointer = Variable.CreatePointer(initial);

        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;
        pointer.Value = initial;

        // create copy
        Assert.That(pointer.Value, Is.EqualTo(initial.Value));

        // change value in one and test it does not propagate to the other
        pointer.Value = 20;
        Assert.That(initial.Value, Is.EqualTo(20));
        Assert.That(pointer.Value, Is.EqualTo(20));

        initial.Value = 30;
        Assert.That(initial.Value, Is.EqualTo(30));
        Assert.That(pointer.Value, Is.EqualTo(30));
    }
}