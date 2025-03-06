using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.InstanceValues;
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
        Variable initial = new Variable("var", new BuiltinTypeInfo<int>().CreateInstance(), VariableModifier.COPY);
        Variable copy = new Variable("copy", new BuiltinTypeInfo<int>().CreateInstance(), VariableModifier.COPY);
        
        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;
        copy.Instance = initial.Instance;
        
        // create copy
        Assert.That(copy.Value, Is.EqualTo(initial.Value));
        
        // change value in one and test it does not propagate to the other
        copy.Value = 20;
        Assert.That(initial.Value, Is.EqualTo(10));
        Assert.That(copy.Value, Is.EqualTo(20));
    }

    [Test]
    public void AssignVariable_Reference()
    {
        // setup variables
        Variable initial = new Variable("var", new BuiltinTypeInfo<int>().CreateInstance(), VariableModifier.COPY);
        Variable copy = new Variable("copy", new BuiltinTypeInfo<int>().CreateInstance(), VariableModifier.REFERENCE);
        
        Assert.That(initial.Value, Is.EqualTo(default(int)));
        initial.Value = 10;
        copy.Instance = initial.Instance;
        
        // create copy
        Assert.That(copy.Value, Is.EqualTo(initial.Value));
        
        // change value in one and test it does not propagate to the other
        copy.Value = 20;
        Assert.That(initial.Value, Is.EqualTo(20));
        Assert.That(copy.Value, Is.EqualTo(20));
    }
}