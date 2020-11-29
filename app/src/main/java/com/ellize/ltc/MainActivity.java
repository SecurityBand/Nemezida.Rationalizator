package com.ellize.ltc;

import androidx.appcompat.app.AppCompatActivity;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.FragmentManager;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;
import androidx.security.crypto.EncryptedSharedPreferences;
import androidx.security.crypto.MasterKeys;

import android.accounts.AccountManager;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.view.View;

import com.ellize.ltc.databinding.ActivityMainBinding;
import com.ellize.ltc.fragfments.MenuFragment;
import com.ellize.ltc.fragfments.NewsIdeasFragment;
import com.ellize.ltc.fragfments.NewsOFfersFragment;
import com.ellize.ltc.fragfments.OfferFragment;

import java.io.IOException;
import java.security.GeneralSecurityException;

public class MainActivity extends AppCompatActivity {
    MainViewModel viewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        createNotificationChannel();
        ActivityMainBinding binding = DataBindingUtil.setContentView(this, R.layout.activity_main);
        viewModel = ViewModelProviders.of(this).get(MainViewModel.class);

        viewModel.dispatcher.observe(this, new Observer<MainViewModel.FRAGMENT>() {
            @Override
            public void onChanged(MainViewModel.FRAGMENT fragment) {
                switch (fragment) {
                    case LOGIN:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, LoginFragment.newInstance()).
                                commit();
                        break;
                    case NEWS_IDEAS:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, NewsIdeasFragment.newInstance("a", ":b")).
                                addToBackStack("").
                                commit();
                        break;
                    case OFFER_ADD:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, OfferFragment.newInstance("1", "2")).
                                addToBackStack("").
                                commit();
                        break;
                    case IDEA_ADD:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, OfferFragment.newInstance("1", "2")).
                                addToBackStack("").
                                commit();
                        break;
                    case NEWS_OFFERS:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, NewsOFfersFragment.newInstance("1", "2")).
                                addToBackStack("").
                                commit();
                        break;
                    case MENU:
                        getSupportFragmentManager().
                                beginTransaction().
                                replace(R.id.fl_container, MenuFragment.newInstance("1", "2")).
                                addToBackStack("").
                                commit();
                        break;
                }

            }
        });

        binding.setLifecycleOwner(this);
        binding.setViewmodel(viewModel);
        if (savedInstanceState == null) {
            AccountManager accountManager = AccountManager.get(this);
            User account = isAccountExist();
            if (account != null) {
                viewModel.user.setValue(account);
                viewModel.login(!account.locale.equals("ru"));
            } else {
                FragmentManager fm = getSupportFragmentManager();
                fm.beginTransaction().replace(R.id.fl_container, LoginFragment.newInstance()).commit();
            }
        }
    }

    User isAccountExist() {
        SharedPreferences prefs = null;
        try {
            prefs = EncryptedSharedPreferences.create("dfsf_e",
                    MasterKeys.getOrCreate(MasterKeys.AES256_GCM_SPEC),
                    getApplication(),
                    EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                    EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM);
        } catch (IOException | GeneralSecurityException e) {
            e.printStackTrace();
        } finally {
            if (prefs != null) {
                String login = prefs.getString("login", null);
                String psw = prefs.getString("psw", null);
                if (login != null && psw != null) {
                    return new User(login, psw);
                }

            }
        }
        return null;
    }

    private void createNotificationChannel() {
        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            CharSequence name = getString(R.string.channel_name);
            String description = getString(R.string.channel_description);
            int importance = NotificationManager.IMPORTANCE_HIGH;
            NotificationChannel channel = new NotificationChannel(Constants.CHANNEL_ID, name, importance);
            channel.setDescription(description);
            // Register the channel with the system; you can't change the importance
            // or other notification behaviors after this
            NotificationManager notificationManager = getSystemService(NotificationManager.class);
            notificationManager.createNotificationChannel(channel);
        }
    }

    public void onClick(View view) {

    }
}