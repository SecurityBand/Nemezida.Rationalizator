package com.ellize.ltc.data;

import java.util.ArrayList;

public class RationalOffer {
    public int id;
    public String title;
    public String description;
    public int rating;
    public String status;
    public String creationDate;
    public String modifiedDate;
    public ArrayList<String> tasg;

    public RationalOffer(String title) {
        this.title = title;
        tasg = new ArrayList<>();
    }
}
