using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace CustomChipNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define network of color chips with various connections
            var chips = new List<ColorChip>
            {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };

            // Compute the longest route from Blue to Green
            var longestRoute = ComputeLongestRoute(chips, Color.Blue, Color.Green);

            // Output the results
            if (longestRoute != null)
            {
                Console.WriteLine("Longest route found:");
                Console.WriteLine("Start Color: {Color.Blue}");
                foreach (var chip in longestRoute)
                {
                    Console.WriteLine($"[{chip.StartColor} → {chip.EndColor}]");
                }
                Console.WriteLine($"End Color: {Color.Green}");
            }
            else
            {
                Console.WriteLine(Messages.NoRouteFound);
            }
        }

        /// Determines longest sequence of color chips connecting the specified start color to the end color
        /// Uses depth-first search with backtracking to explore all possible paths.
        /// <param name="chips">The list of color chips available for the route.</param>
        /// <param name="startColor">The color where the sequence starts.</param>
        /// <param name="endColor">The target color where the sequence ends.</param>
        /// <returns>A list of color chips representing the longest route from start to end, or null if no route is found.</returns>
        /// 

        static List<ColorChip> ComputeLongestRoute(List<ColorChip> chips, Color startColor, Color endColor)
        {
            List<ColorChip> optimalRoute = null;
            var currentRoute = new List<ColorChip>();
            var exploredIndices = new HashSet<int>();

            // Recursive method to explore paths using depth-first search
            void Explore(Color currentColor)
            {
                if (currentColor == endColor)
                {
                    if (optimalRoute == null || currentRoute.Count > optimalRoute.Count)
                    {
                        // If no optimal route found yet or the current route is longer, update the optimal route
                        optimalRoute = new List<ColorChip>(currentRoute);
                    }
                    return;
                }

                for (int i = 0; i < chips.Count; i++)
                {
                    //SKip if chip used in current path
                    if (exploredIndices.Contains(i)) continue;

                    var chip = chips[i];
                    if (chip.StartColor == currentColor)
                    {
                        //Marks chip as used
                        exploredIndices.Add(i);
                        currentRoute.Add(chip);
                        //Using recursion to explore next color
                        Explore(chip.EndColor);
                        currentRoute.RemoveAt(currentRoute.Count - 1);
                        //Marks chip as unused
                        exploredIndices.Remove(i);
                    }
                    else if (chip.EndColor == currentColor)
                    {
                        exploredIndices.Add(i);
                        currentRoute.Add(new ColorChip(chip.EndColor, chip.StartColor));
                        Explore(chip.StartColor);
                        currentRoute.RemoveAt(currentRoute.Count - 1);
                        exploredIndices.Remove(i);
                    }
                }
            }

            // Start exploring paths from the initial color
            Explore(startColor);
            return optimalRoute;
        }
    }

    public enum Color
    {
        Blue,
        Yellow,
        Red,
        Green,
        Orange,
        Purple
    }

    public class ColorChip
    {
        public Color StartColor { get; }
        public Color EndColor { get; }

        public ColorChip(Color start, Color end)
        {
            StartColor = start;
            EndColor = end;
        }
    }

    public static class Messages
    {
        public const string NoRouteFound = "Unable to find a route connecting the specified colors.";
    }
}
