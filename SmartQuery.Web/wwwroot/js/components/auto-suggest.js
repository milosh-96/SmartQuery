// this component requires lodash (tested on version 4.17.21) - make sure it's is available on all pages where this component appears //
const template = document.createElement("template");
template.innerHTML = `
<style>
    .hide {display:none;}
    .show {display:block;}
    input {
        background-image:none;
        background-color:transparent;
        -webkit-box-shadow: none;
        -moz-box-shadow: none;
        box-shadow: none;
    }
    input {font-size:16px;padding:8px;border-radius:8px 8px 8px 8px;}
    .dropdown {
        width:600px;
        border: 1px solid #ccc;
        border-radius: 8px 8px  8px 8px;
        margin:0 10px;
        box-shadow: 2px 2px 2px #ccc;
    }
    .dropdown ul li {padding:8px 8px;}
    .dropdown ul li:hover {background:#ccc;}
    .dropdown ul {list-style-type:none;margin:0;padding:0;}
</style>

<div style="display:flex">
    <div id="input-box">
        <input type="text"  />

    </div>
    <div class="dropdown hide">
        <ul id="dropdown-items">
          

        </ul>
        <button class="btn btn-primary" id="close-dropdown">Close</button>
    </div>
</div>

`;

class AutoSuggest extends HTMLElement {
    data = [];
    selectedItemEventName = "AutoSuggestionItemSelected";
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(template.content.cloneNode(true));
    }
    connectedCallback() {

        if (this.getAttribute("selected-event-name") != null) {
            this.selectedItemEventName = this.getAttribute("selected-event-name");
        }

        let input = this.shadowRoot.querySelector("#input-box").getElementsByTagName("input")[0];
        let closeButton = this.shadowRoot.querySelector("#close-dropdown");
        let dropdownContainer = this.shadowRoot.querySelector(".dropdown");

        // close button event //
        closeButton.addEventListener("click", () => { this.hideDropdown(); });
        // listen for user input //
        input.addEventListener("keyup", async (e) => {
            if (input.value.length >= 3) {
                let displayProperty = this.getAttribute("display-property");
                await this.fetchData(input.value);
                // this array will contain only items that match the user input //
                let filtered = this.data;

                // pass it to render items function //
                this.refreshItems(filtered);

                // display dropdown container //
                dropdownContainer.classList.remove("hide");
                dropdownContainer.classList.add("show");
            }

            // if user clears input or the input length isn't satisfied then remove dropdown container //
            else {

                // hide dropdown container //
                dropdownContainer.classList.remove("show");
                dropdownContainer.classList.add("hide");
            }
        });

        // this is just some demo data (it will be replaced with the data from the api)
        this.data = [
            { id: 1, name: "Miloš" },
            { id: 2, name: "MILOŠ" },
            { id: 3, name: "Милош" },
            { id: 4, name: "МИЛОШ" },
        ];

        // render the demo data 
        this.refreshItems(this.data);

        
    }
    refreshItems(items) {
        let dropdownItems = this.shadowRoot.querySelector("#dropdown-items");
        dropdownItems.innerHTML = ""
        let displayProperty = this.getAttribute("display-property");

        if (items.length > 0) {

            items.map((item) => {
                let liItem = document.createElement("li");
                liItem.innerHTML = _.get(item, displayProperty);
                liItem.addEventListener("click", () => {
                    let event = new CustomEvent(this.selectedItemEventName, { detail: item, bubbles: true, composed: true });
                    this.shadowRoot.dispatchEvent(event);
                });
                dropdownItems.appendChild(liItem);
            })
        }
        else {
            let liItem = document.createElement("li");

            liItem.innerHTML = "There are no items that match this query.";

            dropdownItems.appendChild(liItem);
        }
    }
    async fetchData(query) {
        this.data = [];
        this.refreshItems(this.data);
        let dataUrl = this.getAttribute("data-url");
        console.log(dataUrl);
        //

        await fetch(dataUrl+query).then((response) => response.json()).then((data) => {
            this.data = data;

        });
        this.refreshItems(this.data);
    }

    hideDropdown() {
        let dropdownContainer = this.shadowRoot.querySelector(".dropdown");
        dropdownContainer.classList.remove("show");
        dropdownContainer.classList.add("hide");
    }
}

window.customElements.define('auto-suggest', AutoSuggest);