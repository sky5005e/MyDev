/*
 * ActiveRichTextField.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;




/**
 * 
 */
public class RichTextField extends  ActiveRichTextField{
   public RichTextField(String text, int[] offsets, byte[] attributes, Font[] fonts, int[] foregroundColors, int[] backgroundColors, long style) 
   {
           super(text,offsets,attributes,fonts,foregroundColors,backgroundColors,style);
   }
   FieldChangeListener calllistener = new FieldChangeListener() {
 
              public void fieldChanged(Field field, int context)
              {
                 String phoneNum = ((ActiveRichTextField) field).getText(); // _phonefield is an EditField
                     Invoke.invokeApplication(Invoke.APP_TYPE_PHONE,
                      new PhoneArguments(PhoneArguments.ARG_CALL, phoneNum));     
              }
    };
} 
