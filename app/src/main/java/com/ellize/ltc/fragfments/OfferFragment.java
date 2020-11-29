package com.ellize.ltc.fragfments;

import android.os.Bundle;

import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.MultiAutoCompleteTextView;
import android.widget.TextView;
import android.widget.Toast;

import com.ellize.ltc.MainViewModel;
import com.ellize.ltc.R;
import com.ellize.ltc.data.RationalOffer;
import com.ellize.ltc.data.SpaceTokenizer;
import com.ellize.ltc.data.TagChip;
import com.pchmn.materialchips.ChipsInput;
import com.pchmn.materialchips.model.Chip;

import java.util.ArrayList;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link OfferFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class OfferFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public OfferFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OfferFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static OfferFragment newInstance(String param1, String param2) {
        OfferFragment fragment = new OfferFragment();
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
        View v =  inflater.inflate(R.layout.fragment_offer, container, false);
        final MainViewModel mViewModel = ViewModelProviders.of(requireActivity()).get(MainViewModel.class);
        final LinearLayout cl = v.findViewById(R.id.include);

        final MultiAutoCompleteTextView ed_title = v.findViewById(R.id.ed_offerTitle);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getContext(),
                R.layout.support_simple_spinner_dropdown_item, mViewModel.titleHitns);
        ed_title.setAdapter(adapter);
        ed_title.setTokenizer(new SpaceTokenizer());
        final EditText ed_content = v.findViewById(R.id.editTextTextMultiLine2);
        v.findViewById(R.id.btn_addTags).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                cl.setVisibility(View.VISIBLE);
            }
        });
        TextView tv_rank = v.findViewById(R.id.tv_rank);
        TextView tv_date = v.findViewById(R.id.tv_date);
        //dlg
        final ChipsInput chipsInput= v.findViewById(R.id.chips_input);
        List<TagChip> contactList = new ArrayList<>();
        for(int i=0;i < mViewModel.ideasTags.size();++i){
            contactList.add(new TagChip(mViewModel.ideasTags.get(i)) );
        }
        chipsInput.setFilterableList(contactList);
        v.findViewById(R.id.btn_sendOffer_dlg).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String title = ed_title.getText().toString();
                String content = ed_content.getText().toString();
                List<TagChip> contactsSelected = (List<TagChip>) chipsInput.getSelectedChipList();

                if(TextUtils.isEmpty(title) || TextUtils.isEmpty(content)){
                    Toast.makeText(getContext(),"Заполните заголовок и описание!",Toast.LENGTH_SHORT).show();
                    cl.setVisibility(View.GONE);
                    return;
                }

                RationalOffer rationalOffer = new RationalOffer(title);
                rationalOffer.description = content;
                for(int i=0; i < contactsSelected.size();++i){
                    rationalOffer.tasg.add(contactsSelected.get(i).getLabel());
                }
                mViewModel.sendNewOffer(rationalOffer);
            }
        });

        v.findViewById(R.id.btn_cancel).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                cl.setVisibility(View.GONE);
            }
        });


        return v;
    }
}