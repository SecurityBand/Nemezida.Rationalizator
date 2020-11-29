package com.ellize.ltc;

import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.provider.MediaStore;
import android.util.Base64;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.lifecycle.AndroidViewModel;
import androidx.lifecycle.MutableLiveData;
import androidx.security.crypto.EncryptedSharedPreferences;
import androidx.security.crypto.MasterKeys;

import com.android.volley.AuthFailureError;
import com.android.volley.NoConnectionError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.TimeoutError;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.ellize.ltc.data.IdeaOffer;
import com.ellize.ltc.data.RationalOffer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class MainViewModel extends AndroidViewModel {

    public String[] titleHitns = {"предлагаю","с целью", "ввиду того, что","так как", "не могу больше молчать",
    "я возмущен","а что если нам","было бы не плохо","очевидно, что","давайте сделаем","как говоривал товарищ Ленин", "решить вопрос о", "решить вопрос"};
    private static final String GET_OFFERS = "api/RationalOffers";

    public enum FRAGMENT{LOGIN, NEWS_IDEAS, NEWS_OFFERS, IDEA_ADD, MENU, OFFER_ADD}
    static final String ACCOUNT_TYPE = "com.ellize.ltc.auth";
    private static final String ACCOUNT_TOKEN_TYPE = "access";
    public static final String BASE_URL = "http://ldt-transformation.nemezida.online/";

    public static final String BASE_AUTH = "auth/register";
    private static final String BASE_STATISTIC = "aaa";
    private static final String GET_IDEAS = "api/Ideas";
    private static final String GET_IDEAS_TAGS = "api/Ideas/Tags";
    private static final String POST_OFFER_ADD = "api/RationalOffers/Add";
    private static final String CHECK_WORK_STATE = "/ffff";

    public MutableLiveData<FRAGMENT> dispatcher;
    public MutableLiveData<String> msg;
    public MutableLiveData<User> user;
    public MutableLiveData<Boolean> isAuth;
    public MutableLiveData<JSONObject> qRresult;
    public MutableLiveData<Boolean> isEmailPswError;
    public MutableLiveData<Boolean> isConnectionError;
    public MutableLiveData<Boolean> isNoInternet;
    public MutableLiveData<String> test_log;
    public MutableLiveData<String> uploadAnswer;
    public MutableLiveData<ArrayList<IdeaOffer>> ideas;
    public MutableLiveData<ArrayList<RationalOffer>> offers;
    public ArrayList<String> ideasTags;
    /**
     * detects which category was choosen from CategoriesFragment
     */
    public MutableLiveData<Integer> categoryFragmentTraker;
    public MutableLiveData<Boolean> isSomethingLoading;
    public MutableLiveData<JSONObject> statistics;

    public MainViewModel(@NonNull Application application) {
        super(application);
        ideasTags = new ArrayList<>();
        ideasTags.add("Энергетика");
        ideasTags.add("Софт");
        ideasTags.add("отдел_продаж");
        ideasTags.add("рабочие_моменты");

        dispatcher = new MutableLiveData<>();
        dispatcher.setValue(FRAGMENT.LOGIN);
        Log.d("fragment", "view model created");
        uploadAnswer = new MutableLiveData<>();
        msg = new MutableLiveData<>();
        msg.setValue("empty");
        user = new MutableLiveData<>();
        isSomethingLoading = new MutableLiveData<>();
        isSomethingLoading.setValue(false);
        User cuser = new User("user@example.ru", "123456");
        user.setValue(cuser);
        isAuth = new MutableLiveData<>();
        qRresult = new MutableLiveData<>();
        isConnectionError = new MutableLiveData<>();
        isEmailPswError = new MutableLiveData<>();
        isNoInternet = new MutableLiveData<>();
        test_log = new MutableLiveData<>();
        Log.d("mathod", "value=" + isNoInternet.getValue());
        categoryFragmentTraker = new MutableLiveData<>();
        statistics = new MutableLiveData<>();
        //new api
        ideas = new MutableLiveData<>();
        ideas.setValue(new ArrayList<IdeaOffer>());
        offers = new MutableLiveData<>();
        offers.setValue(new ArrayList<RationalOffer>());
        loadTags();
    }


    public void sendNewOffer(RationalOffer rationalOffer){
        try {
            JSONObject paramsVolley = new JSONObject();
            JSONArray jsonArray = new JSONArray();
            try {
                paramsVolley.put("title",rationalOffer.title);
                paramsVolley.put("description",rationalOffer.description);
                paramsVolley.put("description",rationalOffer.description);
                for(String s:rationalOffer.tasg){
                    jsonArray.put(s);
                }
                paramsVolley.put("Tags",jsonArray);


            } catch (JSONException e1) {
                e1.printStackTrace();
            }
            isSomethingLoading.setValue(true);
            JsonObjectRequest jsonObjReq = new JsonObjectRequest(Request.Method.POST,
                    BASE_URL + POST_OFFER_ADD,
                    paramsVolley,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            isSomethingLoading.setValue(false);
                            Log.d("json", response.toString());
                            Toast.makeText(getApplication(),"Предложение отправлено",Toast.LENGTH_SHORT).show();
                            dispatcher.setValue(FRAGMENT.NEWS_IDEAS);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError e) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, "Error Occurred", Toast.LENGTH_SHORT).show();
//                            if(BuildConfig.DEBUG) {
                            //demo
                            Toast.makeText(getApplication(),"Предложение отправлено",Toast.LENGTH_SHORT).show();
                            dispatcher.setValue(FRAGMENT.NEWS_IDEAS);

                            Log.d("json", e.toString());
//                            Toast.makeText(getApplication(),"Не удалось отправить данные!",Toast.LENGTH_SHORT).show();
//                            }
                            //e.printStackTrace();
                            catchErrorOnLoginConnect(e);
                        }
                    }) {
                @Override
                public Map<String, String> getHeaders() throws AuthFailureError {
                    Map<String, String> headers = new HashMap<>();
//                    String credentials = login + ":" + psw;
//                    String auth = "Basic " + Base64.encodeToString(credentials.getBytes(), Base64.NO_WRAP);
//                    // headers.put("Content-Type", "application/json");
//                    headers.put("Authorization", auth);
//                    headers.put("Content-Type","application/json");
                    return headers;
                }//*/
            };

            VolleySingle.getInstance(getApplication()).addToRequestQueue(jsonObjReq);
        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public void loadTags(){
        String url = BASE_URL+GET_IDEAS_TAGS;
        StringRequest stringRequest =
                new StringRequest(
                        Request.Method.GET,
                        url,
                        new Response.Listener<String>() {
                            @Override
                            public void onResponse(String response) {
                                android.util.Log.d(
                                        "patch", "ideas tags got" + response);
                                try {
                                    JSONObject jsonObject= new JSONObject(response);
                                    JSONArray jsonArray = jsonObject.getJSONArray("items");
                                    ideasTags.clear();
                                    for(int i=0; i < jsonArray.length();++i){
                                        String item = jsonArray.getString(i);
                                        ideasTags.add(item);
                                    }

                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                android.util.Log.d("patch", "ideas tags responce:" + error);
                                if (error.networkResponse != null) {
                                    Log.d(
                                            "patch",
                                            "ideas tags get responce:" + error.networkResponse);
                                    Toast.makeText(
                                            getApplication(),
                                            "Ошибка при запросе данных tags",
                                            Toast.LENGTH_SHORT)
                                            .show();
                                } else {
//                                    Toast.makeText(getApplication(), "Ошибка соединения", Toast.LENGTH_SHORT)
//                                            .show();
                                }
                            }
                        }) {
                    @Override
                    protected Map<String, String> getParams() throws AuthFailureError {
                        Map<String, String> params = new HashMap<String, String>();
//                        params.put("login", arr[0]);
//                        params.put("password", arr[1]);
                        return params;
                    }

                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        Map<String, String> headers = new HashMap<>();
                        String credentials;
                        return headers;
                    }
                };

        VolleySingle.getInstance(getApplication()).addToRequestQueue(stringRequest);

    }

    public void loadIdeas() {
        String url = BASE_URL+GET_IDEAS;
        StringRequest stringRequest =
                new StringRequest(
                        Request.Method.GET,
                        url,
                        new Response.Listener<String>() {
                            @Override
                            public void onResponse(String response) {
                                android.util.Log.d(
                                        "patch", "ideas got" + response);
                                try {
                                    JSONObject jsonObject= new JSONObject(response);
                                    JSONArray jsonArray = jsonObject.getJSONArray("items");
                                    ArrayList<IdeaOffer> arrayList = new ArrayList<>();
                                    for(int i=0; i < jsonArray.length();++i){
                                        JSONObject item = jsonArray.getJSONObject(i);
                                        IdeaOffer ideaOffer = new IdeaOffer(item.getString("text"));
                                        ideaOffer.id = item.getInt("id");
                                        ideaOffer.rating = item.getInt("raiting");
                                        ideaOffer.creationDate = item.getString("createdDate");
                                        JSONArray array = item.getJSONArray("tags");
                                        for(int j =0; j < array.length();++j){
                                            ideaOffer.tasg.add(array.getString(j));
                                        }
                                        arrayList.add(ideaOffer);
                                    }
                                    ideas.setValue(arrayList);
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                android.util.Log.d("patch", "sendOnFrowarding responce:" + error);
                                String s = Constants.DEMO_IDEAS;
                                try {
                                    JSONObject jsonObject = new JSONObject(s);
                                    JSONArray jsonArray = jsonObject.getJSONArray("items");
                                    ArrayList<IdeaOffer> arrayList = new ArrayList<>();
                                    for(int i=0; i < jsonArray.length();++i){
                                        JSONObject item = jsonArray.getJSONObject(i);
                                        IdeaOffer ideaOffer = new IdeaOffer(item.getString("text"));
                                        ideaOffer.id = item.getInt("id");
                                        ideaOffer.rating = item.getInt("raiting");
                                        ideaOffer.creationDate = item.getString("createdDate");
                                        JSONArray array = item.getJSONArray("tags");
                                        for(int j =0; j < array.length();++j){
                                            ideaOffer.tasg.add(array.getString(j));
                                        }
                                        arrayList.add(ideaOffer);
                                    }
                                    ideas.setValue(arrayList);
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                                if (error.networkResponse != null) {
                                    Log.d(
                                            "patch",
                                            "sideas get responce:" + error.networkResponse);
//                                    Toast.makeText(
//                                            getApplication(),
//                                            "Ошибка при запросе данных о клиенте",
//                                            Toast.LENGTH_SHORT)
//                                            .show();
                                } else {
//                                    Toast.makeText(getApplication(), "Ошибка соединения", Toast.LENGTH_SHORT)
//                                            .show();
                                }
                            }
                        }) {
                    @Override
                    protected Map<String, String> getParams() throws AuthFailureError {
                        Map<String, String> params = new HashMap<String, String>();
//                        params.put("login", arr[0]);
//                        params.put("password", arr[1]);
                        return params;
                    }

                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        Map<String, String> headers = new HashMap<>();
                        String credentials;
//
                        return headers;
                    }
                };


        VolleySingle.getInstance(getApplication()).addToRequestQueue(stringRequest);
    }
    public void loadOffers() {
        String url = BASE_URL+GET_OFFERS;
        StringRequest stringRequest =
                new StringRequest(
                        Request.Method.GET,
                        url,
                        new Response.Listener<String>() {
                            @Override
                            public void onResponse(String response) {
                                android.util.Log.d(
                                        "patch", "offers got" + response);
                                try {
                                    JSONObject jsonObject= new JSONObject(response);
                                    JSONArray jsonArray = jsonObject.getJSONArray("items");

                                    ArrayList<RationalOffer> arrayList = new ArrayList<>();
                                    for(int i=0; i < jsonArray.length();++i){
                                        JSONObject item = jsonArray.getJSONObject(i);
                                        RationalOffer ideaOffer = new RationalOffer(item.getString("description"));
                                        ideaOffer.id = item.getInt("id");
                                        ideaOffer.rating = item.getInt("raiting");
                                        ideaOffer.creationDate = item.getString("creationDate");
                                        JSONArray array = item.getJSONArray("tags");
                                        for(int j =0; j < array.length();++j){
                                            ideaOffer.tasg.add(array.getString(j));
                                        }
                                        arrayList.add(ideaOffer);
                                    }
                                    offers.setValue(arrayList);
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                android.util.Log.d("patch", "sendOnFrowarding responce:" + error);
                                if (error.networkResponse != null) {
                                    Log.d(
                                            "patch",
                                            "offers get responce:" + error.networkResponse);
                                    Toast.makeText(
                                            getApplication(),
                                            "Ошибка при запросе данных о клиенте",
                                            Toast.LENGTH_SHORT)
                                            .show();
                                } else {
//                                    Toast.makeText(getApplication(), "Ошибка соединения", Toast.LENGTH_SHORT)
//                                            .show();
                                }
                            }
                        }) {
                    @Override
                    protected Map<String, String> getParams() throws AuthFailureError {
                        Map<String, String> params = new HashMap<String, String>();
//                        params.put("login", arr[0]);
//                        params.put("password", arr[1]);
                        return params;
                    }

                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        Map<String, String> headers = new HashMap<>();
                        String credentials;

                        return headers;
                    }
                };

        VolleySingle.getInstance(getApplication()).addToRequestQueue(stringRequest);
    }
    public void createAccount() {
        SharedPreferences prefs = null;
        try {
            prefs = EncryptedSharedPreferences.create("pref_e",
                    MasterKeys.getOrCreate(MasterKeys.AES256_GCM_SPEC),
                    getApplication(),
                    EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                    EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM);
        } catch (GeneralSecurityException | IOException e) {
            e.printStackTrace();
        } finally {
            if(prefs!=null){
                prefs.edit().putString("login",user.getValue().login).
                putString("psw",user.getValue().psw).commit();

            }
        }
    }

    public void login(boolean isFirstTime) {
//        login(user.getValue().login, user.getValue().psw, getApplication(), isFirstTime);
        isAuth.setValue(true);
        dispatcher.setValue(FRAGMENT.MENU);
    }

    /**
     * method to remove error button in SvanQRFragment and TextView in loginFragment
     */
    private void resetErrors() {
        Log.d("method", "reset errors");
        isNoInternet.setValue(true);
        isConnectionError.setValue(true);
        isEmailPswError.setValue(true);
    }


    /**
     * catch error to show error textview in loginFragment
     */
    private void catchErrorOnLoginConnect(VolleyError e) {
        Log.d("method", "catchErrorOnConnect");
        test_log.setValue(e.toString());
        if (e instanceof NoConnectionError) {
            //Toast.makeText(context, "connection error", Toast.LENGTH_SHORT).show();
            isNoInternet.setValue(false);
        } else if (e instanceof TimeoutError) {
            //Toast.makeText(context, "server timeout error", Toast.LENGTH_SHORT).show();
            isConnectionError.setValue(false);
        } else if (e.networkResponse != null) {
            if (e.networkResponse.statusCode == 403) {
                //Toast.makeText(context, "password error", Toast.LENGTH_SHORT).show();
                isEmailPswError.setValue(false);

            } else if (e.networkResponse.statusCode == 404) {
                //email error
                //Toast.makeText(context, "email error", Toast.LENGTH_SHORT).show();
                isEmailPswError.setValue(false);
            }
        } else {
            isConnectionError.setValue(false);
        }
        isAuth.setValue(false);
    }

    public void checkWorkState(){
        try {
            JSONObject paramsVolley = new JSONObject();
            try {
                paramsVolley.put("login", user.getValue().login);


            } catch (JSONException e1) {
                e1.printStackTrace();
            }
            isSomethingLoading.setValue(true);
            JsonObjectRequest jsonObjReq = new JsonObjectRequest(Request.Method.POST,
                    BASE_URL + BASE_AUTH,
                    paramsVolley,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, response.toString(), Toast.LENGTH_SHORT).show();

                            Log.d("json", response.toString());
                            //test_log.setValue(response.toString());



                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError e) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, "Error Occurred", Toast.LENGTH_SHORT).show();
//                            if(BuildConfig.DEBUG) {
                            Log.d("json", e.toString());
                            test_log.setValue(e.toString());
//                            }
                            //e.printStackTrace();
                            catchErrorOnLoginConnect(e);
                        }
                    }) {
                @Override
                public Map<String, String> getHeaders() throws AuthFailureError {
                    Map<String, String> headers = new HashMap<>();


                    headers.put("Content-Type","application/json");
                    return headers;
                }//*/
            };

            VolleySingle.getInstance(getApplication()).addToRequestQueue(jsonObjReq);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    public void login(final String login, final String psw, final Context context, final boolean isFirstTime) {
        //todo tempory
//        isAuth.setValue(true);//to delete
//        if (true) return; //to delete
        try {
            JSONObject paramsVolley = new JSONObject();
            try {
                paramsVolley.put("login", login);
                paramsVolley.put("password", psw);

            } catch (JSONException e1) {
                e1.printStackTrace();
            }
            isSomethingLoading.setValue(true);
            JsonObjectRequest jsonObjReq = new JsonObjectRequest(Request.Method.POST,
                    BASE_URL + BASE_AUTH,
                    paramsVolley,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, response.toString(), Toast.LENGTH_SHORT).show();

                            Log.d("json", response.toString());
                            //test_log.setValue(response.toString());

                            User currentUser = user.getValue();
                            currentUser.isLogined = true;


//                                currentUser.token = response.getString("token");
//                                currentUser.fio = response.getJSONObject("user").getString("fio");
//                                currentUser.inn = response.getJSONObject("user").getString("inn");
//                                currentUser.company = response.getJSONObject("user").getString("company");
//                                currentUser.avatar_url = response.getJSONObject("user").getString("avatar");
                                //currentUser.locale = getLocale(getApplication());
                                user.postValue(user.getValue());
                                resetErrors();
                                isAuth.setValue(true);
                                /*if(!isFirstTime && !user.getValue().locale.equals(getLocale(getApplication()))){
                                    setLocale2(getLocale(getApplication()));
                                }*/
                                //setLocale2(getLocale(getApplication()));
                                if (isFirstTime) {
                                    createAccount();
                                    //set current app locale
                                    //setLocale(getLocale(getApplication()));
                                    //   setLocale2(getLocale(getApplication()));
                                    //setLocaleAsync(getLocale(getApplication()));
                                }


                            //Toast.makeText(context, "success:" + response.toString(), Toast.LENGTH_SHORT).show();
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError e) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, "Error Occurred", Toast.LENGTH_SHORT).show();
//                            if(BuildConfig.DEBUG) {
                            Log.d("json", e.toString());
                            test_log.setValue(e.toString());
//                            }
                            //e.printStackTrace();
                            catchErrorOnLoginConnect(e);
                        }
                    }) {
                @Override
                public Map<String, String> getHeaders() throws AuthFailureError {
                    Map<String, String> headers = new HashMap<>();
                    String credentials = login + ":" + psw;
                    String auth = "Basic " + Base64.encodeToString(credentials.getBytes(), Base64.NO_WRAP);
                    // headers.put("Content-Type", "application/json");
                    headers.put("Authorization", auth);
                    headers.put("Content-Type","application/json");
                    return headers;
                }//*/
            };

            VolleySingle.getInstance(getApplication()).addToRequestQueue(jsonObjReq);
        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    /***
     * gets whole statistic from web
     * */
    public void getStatistics() {
        try {
            JSONObject paramsVolley = new JSONObject();
            try {
                paramsVolley.put("login", "login");
                paramsVolley.put("password", "psw");

            } catch (JSONException e1) {
                e1.printStackTrace();
            }
            isSomethingLoading.setValue(true);
            JsonObjectRequest jsonObjReq = new JsonObjectRequest(Request.Method.POST,
                    BASE_URL + BASE_STATISTIC,
                    paramsVolley,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, response.toString(), Toast.LENGTH_SHORT).show();
                            Log.d("json", response.toString());
                            //test_log.setValue(response.toString());
                            statistics.setValue(response);
                            //Toast.makeText(context, "success:" + response.toString(), Toast.LENGTH_SHORT).show();
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError e) {
                            isSomethingLoading.setValue(false);
                            //Toast.makeText(context, "Error Occurred", Toast.LENGTH_SHORT).show();
//                            if(BuildConfig.DEBUG) {
                            Log.d("json", e.toString());
                            test_log.setValue(e.toString());
//                            }
                            //e.printStackTrace();
                            catchErrorOnLoginConnect(e);
                        }
                    }) {
                @Override
                public Map<String, String> getHeaders() throws AuthFailureError {
                    Map<String, String> headers = new HashMap<>();
//                    String credentials = login + ":" + psw;
//                    String auth = "Basic " + Base64.encodeToString(credentials.getBytes(), Base64.NO_WRAP);
//                    // headers.put("Content-Type", "application/json");
//                    headers.put("Authorization", auth);
                    return headers;
                }//*/
            };

            VolleySingle.getInstance(getApplication()).addToRequestQueue(jsonObjReq);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static String convertInputStreamToString(InputStream inputStream) throws IOException {
        BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
        String line = "";
        String result = "";
        while ((line = bufferedReader.readLine()) != null)
            result += line;

        inputStream.close();
        return result;

    }


    public void setCurrentFragment(int inx) {
        categoryFragmentTraker.setValue(inx);
    }


    public byte[] getFileDataFromDrawable(Bitmap bitmap) {
        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 80, byteArrayOutputStream);
        return byteArrayOutputStream.toByteArray();
    }

    public String getPath(Uri uri) {
        Cursor cursor = getApplication().getContentResolver().query(uri, null, null, null, null);
        cursor.moveToFirst();
        String document_id = cursor.getString(0);
        document_id = document_id.substring(document_id.lastIndexOf(":") + 1);
        cursor.close();

        cursor = getApplication().getContentResolver().query(
                MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                null, MediaStore.Images.Media._ID + " = ? ", new String[]{document_id}, null);
        cursor.moveToFirst();
        String path = cursor.getString(cursor.getColumnIndex(MediaStore.Images.Media.DATA));
        cursor.close();

        return path;
    }

    public static int getComplementaryColor( int color) {
        int R = color & 255;
        int G = (color >> 8) & 255;
        int B = (color >> 16) & 255;
        int A = (color >> 24) & 255;
        R = 255 - R;
        G = 255 - G;
        B = 255 - B;
        return R + (G << 8) + ( B << 16) + ( A << 24);
    }
}
