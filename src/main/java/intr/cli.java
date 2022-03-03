package intr;
import intr.*;

import java.io.IOException;

public class cli {
    public static void main(String args[]) throws IOException {
    System.out.println(intr.utli.color("CatoScript","green"));
    if(args.length == 0){
        String e = intr.utli.readLine();
        System.out.println(intr.data.get(e));
    }else {
        for(var i = 0; i < args.length; i++){
            System.out.println(args[i]);
        }
    }
    }
}
