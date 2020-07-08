# Mazes for Programmers
This is my implementation of [Jamis Buck](https://github.com/jamis)'s amazing book [Mazes for programmers](https://pragprog.com/titles/jbmaze) using C#.

My main goals where:
- Implement the book as a C# console application.
- Implement the book as a [Blazor WebAssembly](https://blazor.net) application.
- Share as much code as possible between the C# console application and the [Blazor WebAssembly](https://blazor.net) application.
- Host the [Blazor WebAssembly](https://blazor.net) in [GitHub Pages](https://pages.github.com) (still pending :sweat_smile:).

The solution contains the following important items:
- ```Mazes``` project: It is the most important piece of code as it implements the data structures and algorithms described in the book. It is shared between the C# console applications and [Blazor WebAssembly](https://blazor.net) application.
- ```Demos``` folder: Contains several projects implementing the demos described in the book as C# console applications. These demos make use of the ```Mazes``` project.
- ```Mazes.Blazor``` Project: It is the [Blazor WebAssembly](https://blazor.net) implementation of some of the most important parts of the book. It makes use of the ```Mazes``` project.
- ```Mazes.Services``` Project: It implements some services and helper methods used by other projects.
- ```Mazes.Services.Test```: It is a C# console applications used as a container of all the demos. It allows us to select and execute any demo.

It has been an amazing experience to share code between the C# console applications and the [Blazor WebAssembly](https://blazor.net) application, making it very easy to convert as a web application the implementation the book, with very little effort.