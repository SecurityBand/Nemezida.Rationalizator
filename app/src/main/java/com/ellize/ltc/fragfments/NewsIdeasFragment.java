package com.ellize.ltc.fragfments;

import android.graphics.Color;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.provider.SyncStateContract;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import com.ellize.ltc.MainViewModel;
import com.ellize.ltc.R;
import com.ellize.ltc.data.IdeaOffer;

import java.util.ArrayList;
import java.util.Random;


public class NewsIdeasFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public NewsIdeasFragment() {
        // Required empty public constructor
    }


    public static NewsIdeasFragment newInstance(String param1, String param2) {
        NewsIdeasFragment fragment = new NewsIdeasFragment();
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
        View v =  inflater.inflate(R.layout.fragment_news, container, false);
        final MainViewModel mViewModel = ViewModelProviders.of(requireActivity()).get(MainViewModel.class);
        RecyclerView recyclerView = v.findViewById(R.id.rc_ideas);
        recyclerView.setLayoutManager(new LinearLayoutManager(requireActivity()));
        final MyAdapter adapter = new MyAdapter(mViewModel.ideas.getValue());
        recyclerView.setAdapter(adapter);
        mViewModel.loadIdeas();
        mViewModel.ideas.observe(getViewLifecycleOwner(), new Observer<ArrayList<IdeaOffer>>() {
            @Override
            public void onChanged(ArrayList<IdeaOffer> ideaOffers) {
                adapter.data = ideaOffers;
                adapter.notifyDataSetChanged();
            }
        });
        Button createOffer = v.findViewById(R.id.btn_createOffer);
        createOffer.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mViewModel.dispatcher.setValue(MainViewModel.FRAGMENT.OFFER_ADD);
            }
        });
        return v;
    }

    static class MyAdapter extends RecyclerView.Adapter<MyAdapter.MyViewHolder>{
        ArrayList<IdeaOffer> data;
        MyAdapter(ArrayList<IdeaOffer> data){
            this.data = data;
        }
        @NonNull
        @Override
        public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View itemview = LayoutInflater.from(parent.getContext()).inflate(R.layout.rc_item,parent,false);
            return new MyViewHolder(itemview);
        }

        @Override
        public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
            IdeaOffer item = data.get(position);
            holder.tv_text.setText(item.description);
            holder.tv_rank.setText("ранг: " + item.rating);
            StringBuilder stringBuilder = new StringBuilder();
            for(String tag: item.tasg){
                stringBuilder.append(tag);
            }
            holder.tv_tags.setText(stringBuilder.toString());
        }

        @Override
        public int getItemCount() {
            return data.size();
        }

        static public class MyViewHolder extends RecyclerView.ViewHolder {
            public TextView tv_rank, tv_text,tv_tags;

            public MyViewHolder(@NonNull View itemView) {
                super(itemView);
                Random rnd = new Random();
                int color = Color.argb(255, rnd.nextInt(256), rnd.nextInt(256), rnd.nextInt(256));
                int opColor = MainViewModel.getComplementaryColor(color);
                itemView.setBackgroundColor(color);
                tv_rank = itemView.findViewById(R.id.tv_rating);
                tv_tags = itemView.findViewById(R.id.tv_tags);
                tv_text = itemView.findViewById(R.id.tv_ideaText);
                tv_text.setTextColor(opColor);
            }
        }
    }
}