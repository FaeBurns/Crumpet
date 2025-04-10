using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;
using Shared;
using Shared.Exceptions;

namespace Crumpet;

public class ProgramRuntimeHandler
{
    public object RunFile(FileInfo file, string entryPointName, object[] args, Stream? inputStream, Stream? outputStream)
    {
        try
        {
            RootNonTerminalNode programNode = ConstructEncompassingRoot(file);

            TreeWalkingInterpreter interpreter = new TreeWalkingInterpreter(programNode, inputStream, outputStream);
            InterpreterExecutor executor = interpreter.Run(entryPointName, args);
            object result = executor.StepUntilComplete().GetValue()!;
            return result;
        }
        catch (InterpreterException r)
        {
            throw;
        }
        catch (Exception e)
        {
            outputStream ??= Console.OpenStandardError();
            StreamWriter sw = new StreamWriter(outputStream);
            sw.WriteLine("Fatal uncaught exception occured during precompilation/execution");
            sw.WriteLine(e);
            throw;
        }

        return -1;
    }

    private RootNonTerminalNode ConstructEncompassingRoot(FileInfo file)
    {
        RootNonTerminalNode rootParseResult = ParseFile(file);
        
        List<RootNonTerminalNode> rootNodes = new List<RootNonTerminalNode>();
        rootNodes.Add(rootParseResult);
        List<string> filesToParse = new List<string>(GetIncludes(rootParseResult));
        HashSet<string> parsedFiles = new HashSet<string>();
        
        for(int i = 0; i < filesToParse.Count; i++)
        {
            string fileName = filesToParse[i];
            FileInfo target = new FileInfo(Path.Combine(file.Directory?.FullName ?? String.Empty, fileName));
            
            // ensure that include exists
            if (!target.Exists)
                throw new FileNotFoundException(ExceptionConstants.PARSE_FILE_NOT_FOUND.Format(target.FullName));
            
            // skip if the file has already been parsed
            if (parsedFiles.Contains(target.FullName))
                continue;
            else
                parsedFiles.Add(target.FullName);
            
            // try and parse the file
            try
            {
                RootNonTerminalNode parseResult = ParseFile(target);
                rootNodes.Add(parseResult);
                
                // add includes to parse list
                filesToParse.AddRange(GetIncludes(parseResult));
                
            }
            catch (Exception e)
            {
                throw new Exception(ExceptionConstants.PARSE_INNER_EXCEPTION.Format(target.FullName), e);
            }
        }

        // assemble new root terminal from the declarations collected from all the files
        return new RootNonTerminalNode(rootNodes.SelectMany(n => n.Declarations));
    }

    private RootNonTerminalNode ParseFile(FileInfo file)
    {
        if (!file.Exists)
            throw new FileNotFoundException(ExceptionConstants.PARSE_FILE_NOT_FOUND.Format(file.FullName));
        
        using StreamReader reader = file.OpenText();
        string source = reader.ReadToEnd();
        
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(source, file.FullName, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE, CrumpetToken.COMMENT);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));
        NodeWalkingParser<CrumpetToken, RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);
        ParseResult<CrumpetToken, RootNonTerminalNode> parseResult = parser.ParseToRoot(tokens);
        
        if (!parseResult.Success)
            throw new ParserException(
                ExceptionConstants.FAILED_TO_WALK_TREE.Format(
                    parseResult.LastTerminalHit,
                    parseResult.LastTerminalHit.Location),
                parseResult.LastTerminalHit.Location);

        return parseResult.Root!;
    }

    private IEnumerable<string> GetIncludes(RootNonTerminalNode root) => root.Declarations
        .Select(d => d.Variant)
        .OfType<IncludeDeclarationNode>()
        .Select(n => n.Target);
}