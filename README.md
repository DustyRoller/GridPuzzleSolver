# GridPuzzleSolver
Solver for different grid based puzzles.

Currently sudoku and kakuro puzzles are supported.

## Puzzle formats

Puzzles must be passed to the solver in the form of text files, the following sections describe the formats for the files for the different puzzle types.

Examples of puzzle files can be found in GridPuzzleSolver/GridPuzzleSolverUnitTests/TestPuzzle.

### Sudoku puzzle format

A sudoku puzzle should be written out with each row on a new line and columns separated with a '|'. Cells are represented by a single character in the gaps between the '|' and should either contain a number from 1 to 9 or '-' if they are yet to be solved.

An example sudoku puzzle would look like:
<pre>
|-|4|2|-|-|5|-|-|6|
|1|9|7|-|-|-|-|4|-|
|5|6|-|4|-|-|1|-|9|
|8|-|1|3|-|-|2|6|-|
|9|-|-|-|7|1|4|5|-|
|-|-|3|2|5|6|-|-|-|
|-|-|5|-|3|2|7|-|-|
|-|-|4|5|9|-|6|-|-|
|-|-|-|7|6|-|-|8|-|
</pre>
Sudoku puzzle files should be saved with the .sud extension.

## Kakuro puzzle format

A kakuro puzzle should be written out with each row on a new line and columns separated with a '|'. Cells are represented by 5 spaces in the gaps between the '|', and should be in one of the following formats:
* x for dead cells that don't make up part of the puzzle
* - for cells that have yet to be solved
* [column clue value]\[row clue value] for cells containing clues, if only one clue is present then two empty spaces must be used.

An example kakuro puzzle would look like:
<pre>
|  x  |17\  |24\  |  x  |  x  |
|  \16|  -  |  -  |20\  |  x  |
|  \23|  -  |  -  |  -  |15\  |
|  x  |  \23|  -  |  -  |  -  |
|  x  |  x  |  \14|  -  |  -  |
</pre>
Kakuro puzzles files should be saved with the .kak extenion.
