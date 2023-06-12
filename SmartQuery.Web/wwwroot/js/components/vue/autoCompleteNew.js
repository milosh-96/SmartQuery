var items = [{
    id: 1,
    name: "Inter"
},
{
    id: 2,
    name: "Liverpool"
},
{
    id: 3,
    name: "Arsenal"
},
{
    id: 4,
    name: "Gamba Osaka"
},
{
    id: 5,
    name: "Sevilla"
},
{
    id: 6,
    name: "Borussia Dortmund"
},
{
    id: 7,
    name: "Ajax Amsterdam"
},
];

export default {

    props: {
        "dataSourceUrl": {type:String},
        "syncEventName": { type: String },
        "inputLabel": { type: String, default:"Type something in..." },
        "selectMode": { type: Boolean, default: true }
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
            this.selectedItems.push(item);
            this.displayDropdown = false;
            let customEvent = new CustomEvent(this.syncEventName, { detail: { itemsIds: this.selectedItemsIdsList } });
            document.dispatchEvent(customEvent);
            this.input = "";
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
            if (newValue.length > 1) {

                console.log(newValue, oldValue);
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
    template: document.getElementById("auto-complete-new-app")



}