package com.ellize.ltc.fragfments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ellize.ltc.MainViewModel;
import com.ellize.ltc.R;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link MenuFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class MenuFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public MenuFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment MenuFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static MenuFragment newInstance(String param1, String param2) {
        MenuFragment fragment = new MenuFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v =  inflater.inflate(R.layout.fragment_menu, container, false);
        final MainViewModel mViewModel = ViewModelProviders.of(requireActivity()).get(MainViewModel.class);
        v.findViewById(R.id.btn_news_offers).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mViewModel.dispatcher.setValue(MainViewModel.FRAGMENT.NEWS_OFFERS);
            }
        });
        v.findViewById(R.id.btn_news_ideas).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mViewModel.dispatcher.setValue(MainViewModel.FRAGMENT.NEWS_IDEAS);
            }
        });
        v.findViewById(R.id.btn_add_offer).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mViewModel.dispatcher.setValue(MainViewModel.FRAGMENT.OFFER_ADD);
            }
        });
        v.findViewById(R.id.btn_add_idea).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mViewModel.dispatcher.setValue(MainViewModel.FRAGMENT.IDEA_ADD);
            }
        });

        return v;
    }
}