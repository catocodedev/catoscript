package intr;
import intr.*;
public class cli {
    public static void main(String args[]){
    System.out.println(intr.utli.color("CatoScript","green"));
    if(args.length == 0){

    }else {
        for(var i = 0; i < args.length; i++){
            System.out.println(args[i]);
        }
    }
    }
}
