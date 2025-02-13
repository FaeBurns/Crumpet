// https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form

grammar crumpet;
    
root_program        
            : EOF
            | declaration* EOF
            ;
                
declaration 
            : functionDeclaration
            | typeDeclaration
            ;
                
functionDeclaration
            : 'func' type IDENTIFIER LPARAN *parameter* RPARAN statementBody
            ;
            
parameter
            : type IDENTIFIER COMMA?
            ;
            
typeDeclaration
            : 'struct' IDENTIFIER LBRACK typeDeclarationField* RBRACK
            ;
            
typeDeclarationField
            : type IDENTIFIER SEMICOLON
            ;
            
statementBody
            : LBRACK statement* RBRACK
            ;

statement    
            : expressionStatement
            | ifStatement
            | iterationStatement
            | flowStatement
            ;
            
expressionStatement
            : expression? SEMICOLON
            ;

flowStatement
            : ( 'continue' | 'break' | ( 'return' expression? ) ) SEMICOLON
            ;        

ifStatement  
            : 'if' LPARAN expression RPARAN statementBody ('else' statementBody)?
            ;

iterationStatement  
            : 'while' LPARAN expression RPARAN statementBody
            ;
            
type
            : IDENTIFIER
            | IDENTIFIER (PERIOD IDENTIFIER)*
            ;
      
      
// actually the top most expression type
expression
            : assignmentExpression
            ;
            
// topmost expression type?
unaryExpression
            : expressionWithPostfix
            ;
            
expressionWithPostfix
            :  primaryExpression (LINDEX argumentExpressionList RINDEX)?
            ;
            
// usually last element in branch of tree - the first component of an expression that does not contain its type but contains the identifier or value
primaryExpression
            : IDENTIFIER
            | literalConstant
            | LPARAN expression RPARAN
            ;
            
// or expression or (unary = self) 
// use or here as this takes oop precidence
assignmentExpression
            : orExpression
            | unaryExpression EQUALS assignmentExpression
            ;
            
argumentExpressionList
            : expression (COMMA expression)*
            ;
            
literalConstant
            : STRING
            | INT
            | FLOAT
            | BOOL
            ;
            
// conditional/mathematical expressions
// order of operations: OR < AND < XOR < EQUALITY < RELATION < SUM < MULT
orExpression
            : andExpression ('||' andExpression)?
            ;

andExpression
            : exclusiveOrExpression ('&&' exclusiveOrExpression)?
            ;

exclusiveOrExpression
            : equalityExpression ('^' equalityExpression)?
            ; 

equalityExpression
            : relationExpression (('==' | '!=') relationExpression)?
            ;
            
relationExpression
            : sumExpression (( '<' | '<=' | '>=' | '>' ) sumExpression)?
            ;

sumExpression
            : multExpression (('+' | '-') multExpression)? 
            ;

multExpression
            : unaryExpression (( '*' | '/' ) unaryExpression)?
            ;
// end conditional/mathematical expressions

// terminals

IDENTIFIER
            : IdentifierAlpha (IdentifierAlpha | Digit)*
            ;
            
fragment IdentifierAlpha
            : [a-zA-Z_]
            ;
            
fragment Digit
            : [0-9] 
            ;
            
fragment DigitNonZero
            : [1-9]
            ;
            
LPARAN: '(' ;
RPARAN: ')' ;
LBRACK: '{' ;
RBRACK: '}' ;
LINDEX: '[' ;
RINDEX: ']' ;
COMMA: ',' ;
SEMICOLON: ';' ;
PERIOD: '.' ;
EQUALS: '=' ;

BOOL
            : 'true' | 'false' 
            ;

INT
            : DigitNonZero Digit*
            ;

FLOAT // requires at least one digit
            : Digit+ '.' Digit+ 
            ;
            
STRING
            : '"' CHARACTER* '"' 
            ;
            
fragment CHARACTER
            : . ;