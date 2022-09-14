# JsonToMath_Generator

Math Problem Generation, via RNG number set generation, from range and condition/s, and Math Expression Engine (inspired by Brad Robinson's [SimpleExpressionEngine](https://github.com/toptensoftware/SimpleExpressionEngine)). </br>
Intended to be used with external service, providing sets of objects, including all necessary data to generate a math problem set in string arrays. </br>
Written and used with `C#` compiler.

## Usage

* Import source code:
  * `GeneratorParser`:
    * `GeneratorParser.Parse(string[], Random)` - Generate numbers, 1 per set. Pass a Random as source of RNG.
    * `GeneratorParser.BuildSet(string, List<double>, bool)` - Replace tokens in string, from passed set of numbers. Optional: ignore strings starting with `{}`
  * `ParserNode`:
    * `ParserNode.Parse(string)` - Returns result of a math expression, passed as a string.

## Syntax

* Range:
  * Valid: `"0:99"`
  * Invalid: `"0:-99"` - supposed max range is less than the minimal range

* Conditions:
  * Modulo:
    * Valid:
      * `"0:99%(2)"` - 0-99 number, which has modulo 2 equal to 0
      * `"0:99%({0})==2"` - 0-99 number, which has modulo of the previous generated number equal to 2
      * `"0:99%(4&&{0}||9)==2"` - 0-99 number, which has modulo of 4 and the previous generated number, or only 9, equal to 2
    * Invalid:
      * `"0:99%1"` - missing ()
      * `"0:99%{0}"` - missing ()
      * `"0:99%({0}==2)"` - invalid == placement
  * Less, LessOrEqual, Greater, GreaterOrEqual:
    * Valid:
      * `"0:99>(2)"` - 0-99 number, which is greater than 2
      * `"0:99<(2)"` - 0-99 number, which is less than 2
      * `"0:99>=({0})"` - 0-99 number, which is greater than or equal to the first generated number
      * `"0:99<=({0}&&4||9)"` - 0-99 number, which is less than or equal to the first generated number and 4, or only 9
    * Invalid:
      * `"0:99<=1"` - missing ()
      * `"0:99<=(0:99)"` - cannot generate number while already generating number. Too random
      * `"0:99>=(0)&&<=(99)"` - not yet implemented syntax.