package intr;

import java.io.*;

public class utli {
        public static String color(String text, String color){
            switch (color){
                case "green":
                    text = "\u001b[32m" + text + "\u001b[0m";
                    break;
                case "red":
                    text = "\u001b[31m" + text + "\u001b[0m";
                    break;
                case "yellow":
                    text = "\u001b[33m" + text + "\u001b[0m";
                    break;
                case "blue":
                    text = "\u001b[34m" + text + "\u001b[0m";
                    break;
                case "magenta":
                    text = "\u001b[35m" + text + "\u001b[0m";
                    break;
                case "cyan":
                    text = "\u001b[85m" + text + "\u001b[0m";
                    break;
                case "gray":
                    text = "\u001b[90m" + text + "\u001b[0m";
                    break;
                case "white":
                    text = "\u001b[38m" + text + "\u001b[0m";
                    break;
                case "black":
                    text = "\u001b[30m" + text + "\u001b[0m";
                    break;
                default:
                    text = "\u001b[0m" + text;
                    break;
            }
            return text;
        }
    public static void clear(){
        System.out.print("\u001b[2J");
        System.out.print("\u001b[H");
    }
    public static String readLine() throws IOException
    {
        // Enter data using BufferReader
        BufferedReader reader = new BufferedReader(
                new InputStreamReader(System.in));

        // Reading data using readLine
        String name = reader.readLine();
        return name;
    }
}
