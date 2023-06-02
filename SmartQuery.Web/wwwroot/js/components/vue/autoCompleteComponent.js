export default {
    props: {
        dataSourceUrl: { type: String },
        itemSelectedEventName: { type: String, default: 'autoCompleteSelected' },
        itemDisplayProperty: { type: String, default: 'name' },
        itemValueProperty: {type:String,default:'id'} // since whole object is sent in event this property isn't really utilized //
    },
    data() {
        return {
            displayDropdwon: false,
            loading: false,
            userInput: {
                input: "",
                validation: { minimumInputLength: 2 },
            },
            fetchedItems: [

            ]
        }
    },
    methods: {
        itemSelected: function (item) {
            let event = new CustomEvent(this.itemSelectedEventName, { bubbles: true, detail: { item: item } });
            document.dispatchEvent(event);
        },
        search: async function () {
            this.loading = true;
            await fetch(this.dataSourceUrl + '?Query=' + this.userInput.input)
                .then(function (response) {
                    return response.json();
                }).then(function (data) {
                    console.log(data);
                    this.fetchedItems = data;
                    this.loading = false;
                }.bind(this));
        }
    },
    computed: {
      
        validUserInput: async function () {
            if (this.userInput.input.length > this.userInput.validation.minimumInputLength) {
                this.displayDropdwon = true;
                await this.search();
                return true;
            }
            this.displayDropdwon = false;
            return false;
        },
        userInputPlaceholderMessage: function () {
            return "Enter more than " + this.userInput.validation.minimumInputLength + " characters.";
        }

    },
    template: document.getElementById("autoCompleteApp")
}