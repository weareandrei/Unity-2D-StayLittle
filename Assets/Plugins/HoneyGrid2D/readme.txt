# GhostByte Unity Plugin - Grid2D and FlexGrid2D

Thank you for using the GhostByte Unity Plugin! This plugin provides powerful functionality for working with 2D arrays in your Unity game, introducing a new datatype called Grid2D and FlexGrid2D. With these data types, you can easily manage and manipulate 2D arrays, enabling smoother development and enhanced gameplay mechanics.

## Features

- **Grid2D**: A convenient data structure that simplifies the handling of 2D arrays in Unity.
- **FlexGrid2D**: An extension of Grid2D with additional flexibility, allowing dynamic resizing and optimized memory allocation.

## Usage

To integrate the GhostByte Unity Plugin into your project, follow these steps:

1. Download the latest release of the GhostByte Unity Plugin from the [GitHub repository](https://github.com/GhostByte/Unity-Plugin).
2. Import the plugin into your Unity project by either copying the files manually or using the Unity Package Manager.
3. Once imported, you can start using the Grid2D and FlexGrid2D datatypes in your scripts.

Here's a basic example demonstrating the usage of Grid2D:

```csharp
// Import the GhostByte namespace
using GhostByte;

// Create a new Grid2D instance
Grid2D<int> grid = new Grid2D<int>(10, 10);

// Set a value at a specific position
grid.SetElement(3, 5, 42);

// Get the value at a specific position
int value = grid.GetElement(3, 5);

// Print the value
Debug.Log("Value at (3, 5): " + value);
```

For more detailed information on how to utilize the Grid2D and FlexGrid2D datatypes, please refer to the [documentation](https://github.com/GhostByte/Unity-Plugin/wiki) provided with the plugin.

## License

This plugin is released under the [MIT License](https://opensource.org/licenses/MIT), which means you are free to modify and use it in your projects, including commercial use. However, please ensure to provide appropriate attribution to GhostByte by mentioning the plugin's origin and its authorship in your project's credits or acknowledgments.

## Support

For any issues, suggestions, or questions regarding the GhostByte Unity Plugin, please feel free to reach out to us through the [GitHub repository's issue tracker](https://github.com/GhostByte/Unity-Plugin/issues). We will do our best to assist you and provide timely updates.

We hope you find the GhostByte Unity Plugin useful in your game development endeavors. Happy coding!

**GhostByte Team**