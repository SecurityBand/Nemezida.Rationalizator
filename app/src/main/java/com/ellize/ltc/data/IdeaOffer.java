package com.ellize.ltc.data;

import java.util.ArrayList;

public class IdeaOffer {
    public int id;
    public String description;
    public int rating;
    public String creationDate;
    public ArrayList<String> tasg;

    public IdeaOffer(String description) {
        this.description = description;
        tasg = new ArrayList<>();
    }
}
