/*
 * ActivityIndicatorFactory.java
 *
 * Copyright © 1998-2010 Research In Motion Ltd.
 * 
 * Note: For the sake of simplicity, this sample application may not leverage
 * resource bundles and resource strings.  However, it is STRONGLY recommended
 * that application developers make use of the localization features available
 * within the BlackBerry development platform to ensure a seamless application
 * experience across a variety of languages and geographies.  For more information
 * on localizing your application, please refer to the BlackBerry Java Development
 * Environment Development Guide associated with this release.
 */

package com.rim.samples.device.ui.progressindicatordemo;




/**
 * A factory class containing static methods to create ActivityIndicatorView objects
 */
public class ActivityIndicatorFactory
{
    // Prevent instantiation
    private ActivityIndicatorFactory()
    {
    }

    
    /**
     * Creates an ActivityIndicatorView object based on supplied arguments
     * @param viewStyle Field style to create ActivityIndicatorView with
     * @param bitmap Image used to create activity indicator animation
     * @param numFrames The number of frames in the bitmap image
     * @param animationFieldStyle Style bit used to create animation field
     * @param label Label text for the activity indicator
     * @param lableStyle Style bit for the view's label field
     * @return ActivityIndicatorView object
     */
    public static ActivityIndicatorView createActivityIndicator(long viewStyle, Bitmap bitmap, int numFrames, long animationFieldStyle, String label, long lableStyle)
    {
        ActivityIndicatorView view = createActivityIndicator(viewStyle, bitmap, numFrames, animationFieldStyle);
        view.createLabel(label, lableStyle);
        
        return view;
    }


    /**
     * Creates an ActivityIndicatorView object based on supplied arguments
     * @param viewStyle Field style to create ActivityIndicatorView with
     * @param bitmap Image used to create activity indicator animation
     * @param numFrames The number of frames in the bitmap image
     * @param animationFieldStyle Style bit used to create animation field
     * @return ActivityIndicatorView object
     */
    public static ActivityIndicatorView createActivityIndicator(long viewStyle, Bitmap bitmap, int numFrames, long animationFieldStyle)
    {
        ActivityIndicatorView view;
        ActivityIndicatorModel model;
        ActivityIndicatorController controller;

        view = new ActivityIndicatorView(viewStyle);
        model = new ActivityIndicatorModel();
        controller = new ActivityIndicatorController();

        view.setController(controller);
        view.setModel(model);

        controller.setModel(model);
        controller.setView(view);

        model.setController(controller);        

        view.createActivityImageField(bitmap, numFrames, animationFieldStyle);

        return view;
    }


    /**
     * Creates an ActivityIndicatorView object based on supplied arguments
     * @param delegate Manager used as a delegate for layout and focus
     * @param viewStyle Field style to create ActivityIndicatorView with
     * @param bitmap Image used to create activity indicator animation
     * @param numFrames The number of frames in the bitmap image
     * @param animationFieldStyle Style bit used to create animation field
     * @param label Label text for the activity indicator
     * @param lableStyle Style bit for the view's label field
     * @return ActivityIndicatorView object
     */
    public static ActivityIndicatorView createActivityIndicator(Manager delegate, long viewStyle, Bitmap bitmap, int numFrames, long animationFieldStyle,
        String label, long lableStyle)
    {
        ActivityIndicatorView view = createActivityIndicator(delegate, viewStyle, bitmap, numFrames, animationFieldStyle);

        view.createLabel(label, lableStyle);

        return view;
    }

    
    /**
     * Creates an ActivityIndicatorView object based on supplied arguments
     * @param delegate Manager used as a delegate for layout and focus
     * @param viewStyle Field style to create ActivityIndicatorView with
     * @param bitmap Image used to create activity indicator animation
     * @param numFrames The number of frames in the bitmap image
     * @param animationFieldStyle Style bit used to create animation field    
     * @return ActivityIndicatorView object
     */
    public static ActivityIndicatorView createActivityIndicator(Manager delegate, long viewStyle, Bitmap bitmap, int numFrames, long animationFieldStyle)
    {
        ActivityIndicatorView view;
        ActivityIndicatorModel model;
        ActivityIndicatorController controller;

        view = new ActivityIndicatorView(viewStyle, delegate);
        model = new ActivityIndicatorModel();
        controller = new ActivityIndicatorController();

        view.setController(controller);
        view.setModel(model);

        controller.setModel(model);
        controller.setView(view);

        model.setController(controller);        

        view.createActivityImageField(bitmap, numFrames, animationFieldStyle);

        return view;
    }
}
