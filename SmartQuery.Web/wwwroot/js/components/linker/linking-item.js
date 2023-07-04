(function(){
    let template = document.createElement("template");
    template.innerHTML = ` 
    <div>
        <span id="name"></span>
        <span>|</span>
        <span id="remove">X</span>
    </div>
    `;
    
    class LinkingItem extends HTMLElement {
        item = null;
        constructor() {
            super();
            this.attachShadow({ mode: 'open' });
            this.shadowRoot.appendChild(template.content.cloneNode(true));
        }
        connectedCallback() {
            if(this.getAttribute("item") != null){
                this.item = JSON.parse(this.getAttribute("item"));
                this.shadowRoot.getElementById("name").innerHTML = this.item.name;
            }
            this.shadowRoot.getElementById("remove").addEventListener("click", () => {
                this.removeItem();
                this.shadowRoot.getElementById("name").innerHTML = "removed";
            });
        }
        async removeItem() {
            let customEvent = new CustomEvent("LinkerRemoveItemSelected", { detail: this.item, composed: true });
            document.dispatchEvent(customEvent);
        }
       
    }
    window.customElements.define("linker-linking-item",LinkingItem);
})();