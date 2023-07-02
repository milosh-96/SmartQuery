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
        }
        async removeItem() {
            let result = false;
          
            return result;
        }
        async delete(){
           
           
        }
    }
    window.customElements.define("linker-linking-item",LinkingItem);
})();