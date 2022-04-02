package com.example.bongi;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.Color;
import android.hardware.display.DisplayManager;
import android.media.MediaCodec;
import android.media.MediaCodecInfo;
import android.media.MediaFormat;
import android.media.projection.MediaProjection;
import android.media.projection.MediaProjectionManager;
import android.net.LocalSocket;
import android.net.LocalSocketAddress;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.Surface;
import android.view.SurfaceView;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.FrameLayout;
import android.widget.GridLayout;
import android.widget.LinearLayout;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import org.bytedeco.javacv.AndroidFrameConverter;
import org.bytedeco.javacv.Frame;
import org.bytedeco.javacv.OpenCVFrameConverter;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.FileDescriptor;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.channels.Channels;
import java.nio.channels.WritableByteChannel;

public class Sra6 extends AppCompatActivity {

    private AndroidFrameConverter converterToBitmap;
    private OpenCVFrameConverter.ToMat converterToMat;
    LocalSocket videoSocket;
    Button toggleRecording;
    EditText b2;
    FrameLayout framelayout1;
    GridLayout gl;
    private static final String TAG = "ScreenRecordActivity";
    private MediaProjectionManager mediaProjectionManager;
    private MediaProjection mediaProjection;
    private Surface inputSurface;
    private SurfaceView sv;
    private MediaCodec videoEncoder;
    private boolean muxerStarted;
    private static final int REQUEST_CODE_CAPTURE_PERM = 1234;
    private static final String VIDEO_MIME_TYPE = "video/avc";
    private MediaCodec.Callback encoderCallback;
    public static boolean stopThread =false;
    MediaCodec.BufferInfo info2;
    private static BufferedOutputStream osw=null;
    private static FileDescriptor fd;
    GridLayout gridlayout2;
    EditText et;
    String ipaddress;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        int he =  Resources.getSystem().getDisplayMetrics().widthPixels;
        int wi =  Resources.getSystem().getDisplayMetrics().heightPixels;

        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_ACTION_BAR);

        converterToBitmap = new AndroidFrameConverter();
        converterToMat = new OpenCVFrameConverter.ToMat();

        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);

        toggleRecording = new Button(this);
        toggleRecording.setText("Capture Start");
        toggleRecording.setWidth(10);

        framelayout1 = new FrameLayout(this);

        FrameLayout.LayoutParams params = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.WRAP_CONTENT,
                FrameLayout.LayoutParams.WRAP_CONTENT);

// decide upon the positioning of the button //
// you will likely need to use the screen size to position the
// button anywhere other than the four corners
        //params.setMargins(.., .., .., ..);

// use static constants from the Gravity class
        params.gravity = Gravity.CENTER_HORIZONTAL;



        gridlayout2=new GridLayout(this);
        gridlayout2.setColumnCount(2);
        gridlayout2.setRowCount(15);
        gridlayout2.setLayoutParams(new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.WRAP_CONTENT,
                ViewGroup.LayoutParams.WRAP_CONTENT));

        b2 = new EditText(this);
        int x3 = (he*75)/100;
        b2.setWidth(x3);

        b2.setGravity(Gravity.BOTTOM);
        b2.setTextColor(Color.RED);
        b2.setHint("Type Your Message..........");
        b2.setHintTextColor(Color.RED);
        b2.setLayoutParams(new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                ViewGroup.LayoutParams.MATCH_PARENT));


        //GridLayout.Spec row = GridLayout.spec(1 , 1);
        //GridLayout.Spec colspan = GridLayout.spec(1 , 0);
        //GridLayout.LayoutParams gridLayoutParam = new GridLayout.LayoutParams(row , colspan);
        //gridlayout2.addView(b2,gridLayoutParam);



        framelayout1.addView(toggleRecording,params);
        framelayout1.addView(b2);

ipaddress = new String();

        getWindow().addContentView(framelayout1,new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT,
                ViewGroup.LayoutParams.WRAP_CONTENT));



        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.M) {
            new AlertDialog.Builder(this)
                    .setTitle("Error")
                    .setMessage("This activity only works on Marshmallow or later.")
                    .setNeutralButton(android.R.string.ok, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            Sra6.this.finish();
                        }
                    })
                    .show();
            return;
        }

        toggleRecording.setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.M)
            @Override
            public void onClick(View v) {

                ipaddress = b2.getText().toString();

                if (muxerStarted) {
                    try {
                        stopRecording();
                    }catch(IOException ex){


                    }
                    toggleRecording.setText("Start Cap");



                } else {
                    Intent permissionIntent = mediaProjectionManager.createScreenCaptureIntent();
                    startActivityForResult(permissionIntent, REQUEST_CODE_CAPTURE_PERM);

                    toggleRecording.setText("Stop Cap");
                }
            }

        });

        mediaProjectionManager = (MediaProjectionManager) getSystemService(
                Context.MEDIA_PROJECTION_SERVICE);

        encoderCallback = new MediaCodec.Callback() {
            @Override
            public void onInputBufferAvailable(@NonNull MediaCodec codec, int index) {

            }

            @Override
            public void onOutputBufferAvailable(@NonNull MediaCodec codec, int index,
                                                @NonNull MediaCodec.BufferInfo info) {

synchronized (this) {

    encodedData = videoEncoder.getOutputBuffer(index);

    if (info.size != 0) {
        if (muxerStarted) {

            try {

                arr = new byte[encodedData.remaining()];
                encodedData.get(arr);
                t3 = new Thread3();
                t3.execute();

            } catch (Exception exception) {
                exception.printStackTrace();
            }


        }
    }
}
                videoEncoder.releaseOutputBuffer(index, false);

            }

            Thread3 t3;

            @Override
            public void onError(@NonNull MediaCodec codec, @NonNull MediaCodec.CodecException e) {
                Log.e(TAG, "MediaCodec " + codec.getName() + " onError:", e);
            }

            @Override
            public void onOutputFormatChanged(@NonNull MediaCodec codec, @NonNull MediaFormat format) {
                Log.d(TAG, "Output Format changed===============================");
                muxerStarted = true;
            }
        };

    }

    public static ByteBuffer encodedData;

    byte[] arr;

    public void onActivityResult(int requestCode, int resultCode, Intent intent) {
        super.onActivityResult(requestCode, resultCode, intent);
        if (REQUEST_CODE_CAPTURE_PERM == requestCode) {

            toggleRecording.setEnabled(true);

            if (resultCode == RESULT_OK) {

                mediaProjection = mediaProjectionManager.getMediaProjection(resultCode, intent);

                startRecording();

                runthethread=true;

                toggleRecording.setText("CaptureOff");

            } else {
                // user did not grant permissions
                new AlertDialog.Builder(this)
                        .setTitle("Error")
                        .setMessage("Permission is required to record the screen.")
                        .setNeutralButton(android.R.string.ok, null)
                        .show();
            }
        }

    }
    public void startRecording() {

        DisplayMetrics metrics = getResources().getDisplayMetrics();

        int screenWidth = metrics.widthPixels;
        int screenHeight = metrics.heightPixels;

        prepareVideoEncoder(screenWidth, screenHeight);

        t2 = new Thread2();
        t2.execute();
    }

    Thread2 t2;


    @RequiresApi(api = Build.VERSION_CODES.M)
    private void prepareVideoEncoder(int width, int height) {

        MediaFormat format = MediaFormat.createVideoFormat(VIDEO_MIME_TYPE, width, height);
        int frameRate = 25;

        format.setInteger(MediaFormat.KEY_COLOR_FORMAT,
                MediaCodecInfo.CodecCapabilities.COLOR_FormatSurface);
        format.setInteger(MediaFormat.KEY_BIT_RATE, 4000000); // 6Mbps
        format.setInteger(MediaFormat.KEY_FRAME_RATE, frameRate);
        format.setInteger(MediaFormat.KEY_CAPTURE_RATE, frameRate);
        format.setInteger(MediaFormat.KEY_REPEAT_PREVIOUS_FRAME_AFTER, 40000);
        format.setInteger(MediaFormat.KEY_CHANNEL_COUNT, 0);
        format.setInteger(MediaFormat.KEY_I_FRAME_INTERVAL, -1);


        try {

            videoEncoder = MediaCodec.createEncoderByType(VIDEO_MIME_TYPE);
            videoEncoder.configure(format, null, null,
                    MediaCodec.CONFIGURE_FLAG_ENCODE);

            DisplayMetrics metrics = getResources().getDisplayMetrics();
            int screenWidth = metrics.widthPixels;
            int screenHeight = metrics.heightPixels;
            int screenDensity = metrics.densityDpi;

            inputSurface = videoEncoder.createInputSurface();

            mediaProjection.createVirtualDisplay("Recording Display", screenWidth,
                    screenHeight, screenDensity,
                    DisplayManager.VIRTUAL_DISPLAY_FLAG_AUTO_MIRROR/* flags */,
                    inputSurface,
                    null /* callback */, null /* handler */);

            sv = new SurfaceView(this);

            videoEncoder.setCallback(encoderCallback);

            videoEncoder.start();


        } catch (IOException e) {
            releaseEncoders();
        }
    }
    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    private void releaseEncoders() {
            if (muxerStarted) {
            }

            muxerStarted = false;

        if (videoEncoder != null) {
            videoEncoder.stop();
            videoEncoder.release();
            videoEncoder = null;
        }
        if (inputSurface != null) {
            inputSurface.release();
            inputSurface = null;
        }
        if (mediaProjection != null) {
            mediaProjection.stop();
            mediaProjection = null;
        }

    }
    private void stopRecording() throws IOException {

        stopThread = true;
        releaseEncoders();
        try {
            runthethread = false;
            synchronized (this) {
                this.notify();
            }
            //if (t2 != null)
            //  t2.join();
        } finally {
            t2 = null;


            Log.d(TAG, "Thread===============is====================Released");
        }
    }

    @Override
    protected void onDestroy(){

        super.onDestroy();

            releaseEncoders();


        try {
            runthethread = false;
            synchronized (this) {
                this.notify();
            }
            //if (t2 != null)
              //  t2.join();
        } finally {
            t2 = null;


            Log.d(TAG, "Thread===============is====================Released");
        }



    }

    boolean runthethread = false;

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            Intent iin2= new Intent(Sra6.this ,DrawActivity.class);
            startActivity(iin2);
            return true;
        }

        return super.onKeyDown(keyCode, event);
    }
    @Override
    protected void onResume(){
        super.onResume();

    }
Socket soc = null;

    BufferedReader br;
    class Thread2 extends AsyncTask<Void,Void,Void>{
        @Override
        protected Void doInBackground(Void... voids) {
            Log.d("Azam============","In thread 2");

            soc = new Socket();

            try {

                if(ipaddress!=null) {
                    soc.connect(new InetSocketAddress(ipaddress, 27183));


                    // soc.setSoTimeout(30000);
                }
            } catch (IOException e) {

                e.printStackTrace();

            }

            Log.d("STREAM", "Socket Accepted at port================>"+soc.getInetAddress());

            try {

                osw= new BufferedOutputStream(soc.getOutputStream());

            } catch (IOException exception) {
                exception.printStackTrace();

            }
            try {

                br = new BufferedReader(new InputStreamReader(soc.getInputStream()));

            } catch (IOException exception) {
                exception.printStackTrace();
            }


            return null;
            }
    }//End of thread 2



    class Thread3 extends AsyncTask<Void,Void,Void>
    {


        @Override
        protected Void doInBackground(Void... params) {


            if(osw!=null) {

                try {
                    osw.write(arr, 0, arr.length);
                } catch (IOException exception) {
                    exception.printStackTrace();
                }

            }

            return null;

        }
    }//End of thread 2


    }




