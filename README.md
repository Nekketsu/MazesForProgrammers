# Mazes for Programmers
This is my implementation of [Jamis Buck](https://github.com/jamis)'s amazing book [Mazes for programmers](https://pragprog.com/titles/jbmaze) using C#.

## Demo :rocket:
Enjoy the demo here: [MazesForProgrammers](https://nekketsu.github.io/MazesForProgrammers).

## Goals:
- [x] Implement the book as a C# console application.
- [x] Implement the book as a [Blazor WebAssembly](https://blazor.net) PWA application.
- [x] Share as much code as possible between the C# console application and the [Blazor WebAssembly](https://blazor.net) application.
- [x] Host the [Blazor WebAssembly](https://blazor.net) in [GitHub Pages](https://pages.github.com): [MazesForProgrammers](https://nekketsu.github.io/MazesForProgrammers).

## Structure
The source code is in the `src` folder, and the demo, [MazesForProgrammers](https://nekketsu.github.io/MazesForProgrammers), is in the `docs` folder as required by [GitHub Pages](https://pages.github.com).

## Solution
The solution, as described above, is in the `src` folder, and it contains the following important items:
- `Mazes` project: It is the most important piece of code as it implements the data structures and algorithms described in the book. It is shared between the C# console applications and [Blazor WebAssembly](https://blazor.net) application.
- `Demos` folder: Contains several projects implementing the demos described in the book as C# console applications. These demos make use of the `Mazes` project.
- `Mazes.Blazor` Project: It is the [Blazor WebAssembly](https://blazor.net) implementation of some of the most important parts of the book. It makes use of the `Mazes` project.
- `Mazes.Services` Project: It implements some services and helper methods used by other projects.
- `Mazes.Services.Test`: It is a C# console applications used as a container of all the demos. It allows us to select and execute any demo.

## Final thoughts
It has been an awesome experience to share code between the C# the console applications and the [Blazor WebAssembly](https://blazor.net) application, making it very straightforward to convert to a web application the implementation of the book with very little effort.

Please, feel free to send issues or pull requests. Any feedback is always welcome!