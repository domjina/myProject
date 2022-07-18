using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MazeSection {

    /// <summary>
    /// The width of the section.
    /// </summary>
    private byte sectionWidth;

    /// <summary>
    /// The height of the section.
    /// </summary>
    private int sectionHeight;

    /// <summary>
    /// The section data is a 2D array of bytes,
    /// where each byte (0 - 255) represents a tile.
    /// </summary>
    private byte[,] sectionData;

    /// <summary>
    /// Chance for a randomly generated wall.
    /// </summary>
    private readonly float wallChance = 0.2f;

    /// <summary>
    /// <para>List of all generated tiles.
    /// Required to unload all tiles when necessary.
    /// </para>
    /// QUESTION: Why does VS 2022 say the list should be
    /// readonly?
    /// </summary>
    private List<GameObject> tiles = new();

    /// <summary>
    /// To make sure sections generate on top of another, we need to pass this property.
    /// </summary>
    private int lastTileYCoord;

    /// <summary>
    /// Generates a random section.
    /// </summary>
    /// <param name="sectionWidth">Width of the section</param>
    /// <param name="sectionHeight">Height of the section</param>
    /// <param name="type">The type of section</param>
    /// <param name="currentY">Y coordinate to generate from there</param>
    public MazeSection(int sectionWidth, int sectionHeight, short type, int currentY) {

        sectionData = new byte[sectionWidth, sectionHeight];
        this.sectionWidth = (byte)sectionWidth;
        this.sectionHeight = sectionHeight;
        this.lastTileYCoord = currentY;
        switch (type) {
            case 0:
                SECTION_Empty();
                break;
            case 1:
                SECTION_RNDNormal();
                break;
            case 2:
                SECTION_RNDColumn();
                break;
        }
        Populate();
        FinaliseSection();

    }

    /// <returns>The Y coordinate of the last
    /// tile in the section.</returns>
    public int GetLastTileYCoord() {
        return lastTileYCoord;
    }

    /// <summary>
    /// Fills the section data with 0s.
    /// (0 is the empty tile)
    /// </summary>
    private void FillSectionZero() {
        for (int x = 0; x < sectionWidth; x++) {
            for (int y = 0; y < sectionHeight; y++) {
                sectionData[x, y] = 0;
            }
        }
    }

    /// <summary>
    /// Generates a section with continous columns.
    /// </summary>
    /// <param name="columnIndex">The column index (0 to width - 1)</param>
    private void GenerateColumns(byte[] columnIndex) {
        for (int y = 0; y < sectionHeight; y++) {
            for (int c = 0; c < columnIndex.Length; c++) {
                if (columnIndex[c] < sectionWidth) {
                    sectionData[columnIndex[c], y] = 1;
                } else {
                    Debug.Log("Could not create Column!");
                }
            }
        }
    }

    /// <summary>
    /// Generates a single column.
    /// </summary>
    /// <param name="columnIndex">The column index (0 to width - 1)</param>
    private void GenerateColumn(byte columnIndex) {
        for (int y = 0; y < sectionHeight; y++) {
            if (columnIndex < sectionWidth) {
                sectionData[columnIndex, y] = 1;
            } else {
                Debug.Log("Could not create Column!");
            }
        }
    }

    /// <summary>
    /// Generates columns at 0 and width - 1.
    /// These columns act as surrounding walls.
    /// </summary>
    private void GenerateWalls() {
        for (int y = 0; y < sectionHeight; y++) {
            sectionData[0, y] = 1;
            sectionData[sectionWidth - 1, y] = 1;
        }
    }

    /// <summary>
    /// Generates a section with randomly placed walls.
    /// Standard chance of wall is 20%
    /// </summary>
    private void GenerateRandom() {
        for (int y = 0; y < sectionHeight; y++) {
            for (int x = 0; x < sectionWidth; x++) {
                if (Random.value < wallChance) {
                    sectionData[x, y] = 1;
                }
            }
        }
    }

    /// <summary>
    /// Randomly places powerups around the section.
    /// </summary>
    private void Populate() {
        for (int x = 1; x < sectionWidth-1; x++) {
            for (int y = 0; y < sectionHeight; y++) {
                float random = Random.value;
                if (0.9f <= random && random < 0.91f) {
                    sectionData[x, y] = 2;
                } else if (0.91f <= random && random < 0.92f) {
                    sectionData[x, y] = 3;
                } else if (0.92f <= random && random < 0.93f) {
                    sectionData[x, y] = 4;
                } else if (0.93f <= random && random < 0.94f) {
                    sectionData[x, y] = 5;
                } else if (0.94f <= random && random < 0.95f) {
                    sectionData[x, y] = 6;
                } else if (0.96f <= random && random < 0.97f) {
                    sectionData[x, y] = 7;
                } else if (0.97f <= random && random < 0.98f) {
                    sectionData[x, y] = 8;
                } else if (0.98f <= random && random < 0.99f) {
                    sectionData[x, y] = 9;
                } else if (0.99f <= random) {
                    sectionData[x, y] = 10;
                }
            }
        }
    }

    /// <summary>
    /// Generates a section with just walls.
    /// </summary>
    private void SECTION_Empty() {
        FillSectionZero();
        GenerateWalls();
    }

    /// <summary>
    /// Generates a section with continous, randomly placed columns.
    /// </summary>
    private void SECTION_RNDColumn() {
        FillSectionZero();
        GenerateColumns(new byte[] { 0, (byte) (sectionWidth - 1) });
        for (int i = 0; i < 3; i++) {
            GenerateColumn((byte)Random.Range(1, sectionWidth - 2));
        }
    }    

    /// <summary>
    /// Generate a section with walls and randomly placed walls within it.
    /// </summary>
    private void SECTION_RNDNormal() {
        FillSectionZero();
        GenerateWalls();
        GenerateRandom();
    }

    /// <summary>
    /// Creates a tile in the scene.
    /// </summary>
    /// <param name="type">The type of tile (0 - 255)</param>
    /// <param name="coordinate">The place of the tile</param>
    private void CreateTile(byte type, Vector3 coordinate) {
        switch (type) {
            case 0:
                break;
            case 1:
                GameObject wall = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/Wall", typeof(GameObject)));
                wall.transform.position = coordinate;
                tiles.Add(wall);
                break;
            case 2:
                GameObject rest = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/Resistance", typeof(GameObject)));
                rest.transform.position = coordinate;
                tiles.Add(rest);
                break;
            case 3:
                GameObject coil = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/Coil", typeof(GameObject)));
                coil.transform.position = coordinate;
                tiles.Add(coil);
                break;
            case 4:
                GameObject battery = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/Battery", typeof(GameObject)));
                battery.transform.position = coordinate;
                tiles.Add(battery);
                break;
            case 5:
                GameObject brokenCircuit = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/BrokenCircuitry", typeof(GameObject)));
                brokenCircuit.transform.position = coordinate;
                tiles.Add(brokenCircuit);
                break;
            case 6:
                GameObject positronSpawner = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/PositronSpawner", typeof(GameObject)));
                positronSpawner.transform.position = coordinate;
                positronSpawner.GetComponent<PositronSpawner>().SetDirection(
                    new Vector3(Random.Range(-1,2), Random.Range(-1,2), 0));
                tiles.Add(positronSpawner);
                break;
            case 7:
                GameObject beamTrigger = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/BeamTrigger", typeof(GameObject)));
                beamTrigger.transform.position = coordinate;
                tiles.Add(beamTrigger);
                break;
            case 8:
                GameObject led = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/LED", typeof(GameObject)));
                led.transform.position = coordinate;
                tiles.Add(led);
                break;
            case 9:
                GameObject shield = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/Shield", typeof(GameObject)));
                shield.transform.position = coordinate;
                tiles.Add(shield);
                break;
            case 10:
                GameObject ghostTrigger = (GameObject)Object.Instantiate(
                        Resources.Load("Objects/GhostTrigger", typeof(GameObject)));
                ghostTrigger.transform.position = coordinate;
                tiles.Add(ghostTrigger);
                break;
        }
    }

    /// <summary>
    /// Translate the section data to Tile GameObjects in the scene.
    /// </summary>
    private void FinaliseSection() {
        for (int y = 0; y < sectionHeight; y++) {
            for (int x = 0; x < sectionWidth; x++) {
                CreateTile(sectionData[x, y], new Vector3(x - sectionWidth / 2, lastTileYCoord, 0));
            }
            lastTileYCoord++;
        }
    }

    /// <summary>
    /// Destroys/unload all tiles.
    /// </summary>
    public void DestroySection() {
        foreach (GameObject wall in tiles) {
            Object.Destroy(wall);
        }
    }

    /// <summary>
    /// Left as debug function. Prints the sectionData in a nice array.
    /// </summary>
    private void PrintSection() {
        string output = "";
        for (int x = 0; x < sectionWidth; x++) {
            for (int y = 0; y < sectionHeight; y++) {
                output += sectionData[x,y];
            }
            output += "\n";
        }
    }

    /// <summary>
    /// Generates a section based on a section blueprint.
    /// </summary>
    /// <param name="sectionBlueprint">
    /// A text file with tile information.
    /// One character is one tile, beginning from the zero.
    /// </param>
    /// <param name="yCoord">
    /// The Y coordinate to generate the section from.
    /// </param>
    public MazeSection(string sectionBlueprint, int yCoord) {

        lastTileYCoord = yCoord;
        TranslateFile(sectionBlueprint);
        FinaliseSection();

    }

    /// <summary>
    /// Reads the section blueprint file and
    /// creates the corresponding section data.
    /// </summary>
    /// <param name="sectionBlueprint">The section blueprint.</param>
    /// <returns>The section data.</returns>
    private List<string> ReadTextFile(string sectionBlueprint) {

        int stringLength = -1;

        // Resource paths in unity start from the Assets Folder
        // e.g. "Assets/Objects/Something.prefab" becomes
        // "Objects/Something"
        string filepath = "Blueprints/" + sectionBlueprint;

        // Unity treats text files as its own form of asset.
        TextAsset textFile = Resources.Load(filepath) as TextAsset;

        // Read the text file as an entire string.
        string text = textFile.text;

        // Split the text string with every enter.
        List<string> input = new(text.Split("\n"));

        // This foreach loop is to validate the blueprint.
        // You might not fuck up but hell I do!
        foreach (string line in input) {
            if (stringLength == -1) {
                stringLength = line.Length;
            } else if (line.Length != stringLength) {
                Debug.Log("Section Blueprint faulty! Variant width.");
                return null;
            } else { }
        }
        return input;
    }

    /// <summary>
    /// Reads a section blueprint text file and
    /// translate the content of it to section data.
    /// </summary>
    /// <param name="sectionBlueprint">The section blueprint.</param>
    private void TranslateFile(string sectionBlueprint) {
        List<string> input = ReadTextFile(sectionBlueprint);

        // Failsafe: In case the blueprint does not anything.
        if (input == null) {
            sectionHeight = 0;
        }

        sectionHeight = input.Count;
        sectionWidth = (byte) input[0].Length;
        sectionData = new byte[sectionWidth, sectionHeight];
        for (int y = 0; y < sectionHeight; y++) {
            string line = input[y];
            byte[] byteLine = Encoding.ASCII.GetBytes(line);
            for (int x = 0; x < sectionWidth; x++) {
                sectionData[x, y] = (byte)(byteLine[x]-48);
            }
        }
    } 

}


