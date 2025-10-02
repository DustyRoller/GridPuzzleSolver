# GridPuzzleSolver
Solver for different grid based puzzles.

Currently sudoku and kakuro puzzles are supported.

## Puzzle formats

Puzzles can be passed to the solver in the form of either text or XML files, the following sections describe the formats for the files for the different puzzle types.

Example puzzle can be found in the [system tests puzzle directory](GridPuzzleSolverSystemTests/TestPuzzles).

### Sudoku puzzle format

#### Text file format

A sudoku puzzle should be written out with each row on a new line and columns separated with a '|'. Cells are represented by a single character in the gaps between the '|' and should either contain a number from 1 to 9 or '-' if they are yet to be solved.

An example sudoku puzzle would look like:
```
|-|4|2|-|-|5|-|-|6|
|1|9|7|-|-|-|-|4|-|
|5|6|-|4|-|-|1|-|9|
|8|-|1|3|-|-|2|6|-|
|9|-|-|-|7|1|4|5|-|
|-|-|3|2|5|6|-|-|-|
|-|-|5|-|3|2|7|-|-|
|-|-|4|5|9|-|6|-|-|
|-|-|-|7|6|-|-|8|-|
```
Sudoku puzzle files should be saved with the .sud extension.

#### XML file format

The XML definition for a sudoku puzzle is shown below. Cell values that have already been provided are defined using the value attribute,
any cell without the value attribute is treated as being an empty cell.

```xml
<SudokuPuzzle>
  <Cells>
    <Cell>
      <Coordinates x="0" y="0" />
    </Cell>
    <Cell value="4">
      <Coordinates x="0" y="1" />
    </Cell>
    <Cell value="2">
      <Coordinates x="0" y="2" />
    </Cell>
    ....
  </Cells>
</SudokuPuzzle>
```

## Kakuro puzzle format

#### Text file format

A kakuro puzzle should be written out with each row on a new line and columns separated with a '|'. Cells are represented by 5 spaces in the gaps between the '|', and should be in one of the following formats:
* x for dead cells that don't make up part of the puzzle
* - for cells that have yet to be solved
* [column clue value]\[row clue value] for cells containing clues, if only one clue is present then two empty spaces must be used.

An example kakuro puzzle would look like:
```
|  x  |17\  |24\  |  x  |  x  |
|  \16|  -  |  -  |20\  |  x  |
|  \23|  -  |  -  |  -  |15\  |
|  x  |  \23|  -  |  -  |  -  |
|  x  |  x  |  \14|  -  |  -  |
```
Kakuro puzzles files should be saved with the .kak extenion.

#### XML file format

The XML definition for a kakuro puzzle is shown below. Cells with the blank attribute set to true are treated as non-puzzle cells,
clue values are defined using the column_clue or row_clue attribute as required. A Cell without any attribute are treated as being empty.

```xml
<KakuroPuzzle>
  <Cells>
    <Cell blank="true">
      <Coordinates x="0" y="0" />
    </Cell>
    <Cell>
      <Coordinates x="0" y="1" />
    </Cell>
    <Cell column_clue="20">
      <Coordinates x="0" y="2" />
    </Cell>
    <Cell row_clue="26">
      <Coordinates x="0" y="3" />
    </Cell>
    <Cell column_clue="12" row_clue="4">
      <Coordinates x="0" y="4" />
    </Cell>
    ...
  </Cells>
</KakuroPuzzle>
```