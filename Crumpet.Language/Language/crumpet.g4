// https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form

grammar crumpet;

// done
root_program
            : declaration*
            ;

// done
declaration
            : functionDeclaration
            | typeDeclaration
            ;

// done
functionDeclaration
            : 'func' type IDENTIFIER LPARAN parameterList* RPARAN statementBody
            ;

// done
parameterList
            : parameter (COMMA parameter)*
            ;

// done
parameter
            : type REFERENCE? IDENTIFIER
            ;

// done
typeDeclaration
            : 'struct' IDENTIFIER LBRACK typeDeclarationField* RBRACK
            ;

// done
typeDeclarationField
            : type REFERENCE? IDENTIFIER SEMICOLON
            ;

// done
statementBody
            : LBRACK statement* RBRACK
            ;

// done
statement
            : expression? SEMICOLON
            | ifStatement
            | iterationStatement
            | flowStatement
            | initializationStatement
            ;

// done
flowStatement
            : ( 'continue' | 'break' | ( 'return' expression? ) ) SEMICOLON
            ;

// done
ifStatement
            : 'if' LPARAN expression RPARAN statementBody ('else' statementBody)?
            ;

// done
iterationStatement
            : 'while' LPARAN expression RPARAN statementBody
            ;

// done
initializationStatement
            : IDENTIFIER REFERENCE? IDENTIFIER SEMICOLON
            ;

// done
type
            : IDENTIFIER (PERIOD IDENTIFIER)*
            ;


// actually the top most expression type
// done
expression
            : assignmentExpression
            ;

// topmost expression type?
// done
unaryExpression
            : expressionWithPostfix
            ;

// done
expressionWithPostfix
            : primaryExpression (LINDEX expression RINDEX)?
            | primaryExpression LPARAN argumentExpressionList? RPARAN
            ;

// usually last element in branch of tree - the first component of an expression that does not contain its type but contains the identifier or value
// done
primaryExpression
            : IDENTIFIER
            | literalConstant
            | LPARAN expression RPARAN
            ;

// or expression or (unary = self)
// use or here as this takes order of operations precidence
// done
assignmentExpression
            : orExpression
            | unaryExpression EQUALS assignmentExpression
            ;

// done
argumentExpressionList
            : expression (COMMA expression)*
            ;

// done
literalConstant
            : STRING
            | INT
            | FLOAT
            | BOOL
            ;

// conditional/mathematical expressions
// order of operations: OR < AND < XOR < EQUALITY < RELATION < SUM < MULT
// done
orExpression
            : andExpression ('||' andExpression)?
            ;

// done
andExpression
            : exclusiveOrExpression ('&&' exclusiveOrExpression)?
            ;

// done
exclusiveOrExpression
            : equalityExpression ('^' equalityExpression)?
            ;

// done
equalityExpression
            : relationExpression (('==' | '!=') relationExpression)?
            ;

// done
relationExpression
            : sumExpression (( '<' | '<=' | '>=' | '>' ) sumExpression)?
            ;

// done
sumExpression
            : multExpression (('+' | '-') multExpression)?
            ;

// done
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
REFERENCE: '&' ;

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