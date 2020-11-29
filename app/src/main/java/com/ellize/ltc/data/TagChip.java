package com.ellize.ltc.data;

import android.graphics.drawable.Drawable;
import android.net.Uri;

import com.pchmn.materialchips.model.ChipInterface;

public class TagChip implements ChipInterface {
    String title;
    public TagChip(String tag){
        title = tag;
    }
    @Override
    public Object getId() {
        return null;
    }

    @Override
    public Uri getAvatarUri() {
        return null;
    }

    @Override
    public Drawable getAvatarDrawable() {
        return null;
    }

    @Override
    public String getLabel() {
        return title;
    }

    @Override
    public String getInfo() {
        return title;
    }
}
