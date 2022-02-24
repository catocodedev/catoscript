package intr;

public class data {
    public static String get(String item){
        String ver = "JDev 0.1.0";
        switch (item){
            case "version":
               return ver;
            default:
                return "Invalid Option";
        }
    }
}
