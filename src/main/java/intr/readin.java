package intr;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.file.*;
import java.util.Scanner;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class readin {
    public static void cato(String file) throws IOException, URISyntaxException {
        FileSystem filesystem = FileSystems.getDefault();
        Scanner scanner = new Scanner(new File(file));
        scanner.useDelimiter(" ");

        scanner.close();
    }
}
