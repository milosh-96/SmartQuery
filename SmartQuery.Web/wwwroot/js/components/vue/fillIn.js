

export default {

    props: {
        "syncEventName": {type:String},
        "dataSourceUrl": {type:String},
        "inputLabel": { type: String, default:"Type something in..." },        

    },
    data() {
        return {
           
            items: [],
            selectedItems: [],
            displayDropdown: false,
            input: ""
        }
    },
    methods: {
        itemSelected: function (item) {
            this.displayDropdown = false;
            //this.input = "";
        }
    },
    computed: {
        dropdownClass: function () {
            if (this.displayDropdown) {
                return "";
            }
            return "d-none";
        },
        selectedItemsIdsList: function () {
            let ids = this.selectedItems.map(s => s.id);
            if (ids.length > 1) {
                return ids.join(",");
            }
            // if there are no items or just one then we can join without a separator
            return ids.join("");
        }
    },
    watch: {
        items: function (newValue, oldValue) {

        },
        input: async function (newValue, oldValue) {
            let customEvent = new CustomEvent(this.syncEventName, { detail: { input: newValue } });
            document.dispatchEvent(customEvent);
            if (newValue.length > 1) {

               //console.log(newValue, oldValue);
                var source = await fetch(this.dataSourceUrl + '?Query=' + newValue)
                    .then(function (response) {
                        return response.json();
                    }).then(function (data) {
                        console.log(data);
                        return data;
                    }.bind(this));
                var filtered = source.filter(item => item.name.toLowerCase().includes(newValue.toLowerCase()));
                this.items = filtered;
                if (this.items.length > 0) {
                    this.displayDropdown = true;
                }
                else {
                    this.items = [];
                    this.displayDropdown = false;

                }
            }
            else {
                this.displayDropdown = false;
            }
            
        }

    },
    template: document.getElementById("fill-in-app")



}