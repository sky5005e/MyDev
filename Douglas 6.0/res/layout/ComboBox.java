/*
 * ComboBox.java
 *
 * © <your company here>, <year>
 * Confidential and proprietary.
 */

package res.layout;

import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.*;
import net.rim.device.api.ui.*;
import net.rim.device.api.system.*; 


public class ComboBox extends ObjectChoiceField {
    private final int PADDING_HEIGHT = 20;
    private final int width;
    private final int height;

    public ComboBox(Object[] choices, int width) {
        super("", choices, 0, FIELD_LEFT);
        this.width = width;
        this.height = getFont().getHeight() + PADDING_HEIGHT;
        this.setMinimalWidth(width);
    }

    public int getPreferredHeight() {
        return height;
    }

    public int getPreferredWidth() {
        return width;
    }

    protected void layout(int w, int h) {
        setExtent(width, height);
    }

    public void getFocusRect(XYRect rect) {
        rect.set(getFont().getAdvance(getLabel()), 0, width, height);
    }

}

