package com.bb.upside;

import com.upside2go.helpers.Upside2goFontManager;

import net.rim.device.api.ui.Color;
import net.rim.device.api.ui.Field;
import net.rim.device.api.ui.FieldChangeListener;
import net.rim.device.api.ui.Graphics;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.ButtonField;
import net.rim.device.api.ui.component.RichTextField;
import net.rim.device.api.ui.container.PopupScreen;
import net.rim.device.api.ui.container.VerticalFieldManager;

public class VideoPopupScreen extends PopupScreen implements FieldChangeListener{
	Upside2goFontManager fontManager = new Upside2goFontManager();
	private VerticalFieldManager _vfManager;
	private RichTextField _rtField;
	private ButtonField _forgetButton;
	private ButtonField _tryAgainButton;
	
	public VideoPopupScreen(){
		super(new VerticalFieldManager(),NO_VERTICAL_SCROLL);
		_vfManager = new VerticalFieldManager(){
			public void paint(Graphics g){
				g.setBackgroundColor(Color.BLACK);
				g.setGlobalAlpha(255);
				g.clear();
				super.paint(g);
			}
		};
		_rtField = new RichTextField("Video jetzt abspielen?",RichTextField.TEXT_ALIGN_HCENTER | RichTextField.NON_FOCUSABLE){
			public void paintBackground(Graphics g){
				g.setBackgroundColor(Color.WHITE);
				g.setGlobalAlpha(0);
				g.clear();
				super.paint(g);
			}
			public void paint(Graphics g){
				g.setColor(Color.WHITE);
				g.setGlobalAlpha(255);
				super.paint(g);
			}
		};
		

		_rtdField = new RichTextField("Durch das Abspielen der Videos können möglicherweise weitere Kosten für Sie entstehen. Bitte überprüfen Sie Ihren Datentarif!",RichTextField.TEXT_ALIGN_HCENTER | RichTextField.NON_FOCUSABLE){
			public void paintBackground(Graphics g){
				g.setBackgroundColor(Color.WHITE);
				g.setGlobalAlpha(0);
				g.clear();
				super.paint(g);
			}
			public void paint(Graphics g){
				g.setColor(Color.WHITE);
				g.setGlobalAlpha(255);
				super.paint(g);
			}
		};
		_forgetButton = new ButtonField("Ja",ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
		
		_forgetButton.setChangeListener(this);
		
		_tryAgainButton = new ButtonField("Nein",ButtonField.CONSUME_CLICK | ButtonField.FIELD_HCENTER);
		_tryAgainButton.setChangeListener(this);
		
		_rtField.setFont(fontManager.MEDIUM_FONT);
		_forgetButton.setFont(fontManager.MEDIUM_FONT);
		_tryAgainButton.setFont(fontManager.MEDIUM_FONT);
		
		_vfManager.add(_rtField);
		_vfManager.add(_rtdField);
		_vfManager.add(_forgetButton);
		_vfManager.add(_tryAgainButton);
		add(_vfManager);
	}
	public void fieldChanged(Field field,int context){
		if (field == _forgetButton){
			UiApplication.getUiApplication().invokeLater(new Runnable(){
				public void run(){
					UiApplication.getUiApplication().popScreen(getScreen());
//					LoginScreen login = new LoginScreen();
//					UiApplication.getUiApplication().popScreen(login);
					ForgotPasswordScreen forgot = new ForgotPasswordScreen();
					UiApplication.getUiApplication().pushScreen(forgot);
				}
			});
			
		} else if (field == _tryAgainButton){
			UiApplication.getUiApplication().invokeLater(new Runnable(){
				public void run(){
					UiApplication.getUiApplication().popScreen(getScreen());
				}
			});
			
		}
	}
	
}
