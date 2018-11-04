/*
 * ActiveRichTextField.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;
import net.rim.blackberry.api.invoke.*;




/**
 * 
 */
public class RichTextField extends  HorizontalFieldManager implements FieldChangeListener{
   private ActiveRichTextField telno;

   public HorizontalFieldManager RichTextField(String _text, int[] _offsets, byte[] _attributes, Font[] _fonts, int[] _foregroundColors, int[] _backgroundColors, long _style){
         
       text = _text;
       offsets= _offsets;
       attributes= _attributes;
       fonts=_fonts;
       foregroundColors=_foregroundColors;
       backgroundColors=_backgroundColors;
       style=_style;
       //Add Footer
        HorizontalFieldManager hfmnew=new HorizontalFieldManager();
      
        Font myFont = FontFamily.forName("BBAlpha Sans").getFont(Font.PLAIN, defaultTextHeight);
        String labelEmailText = flag==1?EventsScreenDetail.store[EventsScreenDetail.selectedIndex].getShop().getTelefon():PerfumerieScreen.store[PerfumerieScreen.selectedIndex].getShop().getTelefon();
        int[] offsets = {0, labelEmailText.length()};
        Font[] fonts = {myFont};
        byte[] attributes = {0};
        ActiveRichTextField telno = new ActiveRichTextField(labelEmailText,offsets, attributes, fonts, null, null,FIELD_LEFT | FOCUSABLE);
        telno.setChangeListener(this);
        telno.setPadding(4,0,0,0);
        hfmnew.add(telno);
       
}
        public void fieldChanged(Field field, int context)
        {
               if(field==telno)
               {
                 String phoneNum = text.getText(); // _phonefield is an EditField
                 Invoke.invokeApplication(Invoke.APP_TYPE_PHONE,
                 new PhoneArguments(PhoneArguments.ARG_CALL, phoneNum));     
               }
        }
    
} 
