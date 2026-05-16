package com.saaritech.keyboard;
import android.app.Activity;
import android.graphics.Rect;
import android.util.DisplayMetrics;
import android.view.View;
import android.view.WindowManager;
import android.view.ViewTreeObserver;
public class SaariTechKeyboard
{
    private static Activity activity;
    private static int keyboardHeight = 0;
    private static int keyboardWidth = 0;
    public static void initialize(Activity _activity)
	{
        activity = _activity;
        final View rootView = activity.getWindow().getDecorView().getRootView();
        rootView.getViewTreeObserver().addOnGlobalLayoutListener(() ->
        {
            Rect rect = new Rect();
            rootView.getWindowVisibleDisplayFrame(rect);
            int screenHeight = rootView.getHeight();
            int heightDiff = screenHeight - rect.bottom;
            keyboardHeight = heightDiff > screenHeight / 4 ? heightDiff : 0;
            keyboardWidth = rect.width();
        });
    }
    public static int Height()
	{
        return keyboardHeight;
    }
    public static int Width()
	{
        return keyboardWidth;
    }
}