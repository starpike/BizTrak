<template>
    <div class="field px-3">
        <label class="label">Materials:</label>
        <div class="control">
            <table class="table responsive-table is-fullwidth is-striped is-hoverable">
                <thead>
                    <tr>
                        <th>
                            <span>Item Name:</span>
                        </th>
                        <th>
                            <span>Qty:</span>
                        </th>
                        <th>
                            <span>Unit Price&nbsp;(£):</span>
                        </th>
                        <th>
                            <span>Cost:</span>
                        </th>
                        <th>
                            <span>Actions:</span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(material, index) in materials" :key="material.id">
                        <td data-label="Item:">
                            <input type="text" class="input" v-model="material.material_name"
                                :class="{ 'is-danger': v.$each.$response.$data[index].material_name.$error }" />
                            <span class="help is-danger"
                                v-for="error in v.$each.$response.$errors[index].material_name" :key="error">
                                {{ error.$message }}
                            </span>
                        </td>
                        <td data-label="Qty:">
                            <input type="number" min="1" max="100" step="1" class="input" v-model="material.quantity"
                                :class="{ 'is-danger': v.$each.$response.$data[index].quantity.$error }" />
                            <span class="help is-danger"
                                v-for="error in v.$each.$response.$errors[index].quantity" :key="error">
                                {{ error.$message }}
                            </span>
                        </td>
                        <td data-label="Unit Price:"><input type="number" min="0" step="0.01" placeholder="0.00"
                                class="input" style="width: 100px;" v-model="material.unit_price"
                                :class="{ 'is-danger': v.$each.$response.$data[index].unit_price.$error }" />
                            <span class="help is-danger"
                                v-for="error in v.$each.$response.$errors[index].unit_price" :key="error">
                                {{ error.$message }}
                            </span>
                        </td>
                        <td data-label="Cost:" class="has-text-right">
                            <span>£{{ (material.quantity * material.unit_price).toFixed(2) }}</span>
                        </td>
                        <td class="is-vtop">
                            <div v-if="material.id">
                                <label class="checkbox no-wrap">
                                    <input type="checkbox" v-model="material.isToBeDeleted">
                                    <span class="is-hidden-mobile"> Delete</span>
                                    <span class="is-hidden-desktop is-hidden-tablet"> Del</span>
                                </label>
                            </div>
                            <div v-else>
                                <button class="button mt-1" @click="undoMaterial(index)" aria-label="Undo Add"
                                    title="Undo Add">
                                    <i class="fas fa-undo"></i>
                                </button>
                            </div>
                        </td>
                    </tr>
                    <tr v-if="!materials || materials.length === 0">
                        <td class="empty" colspan="5">No materials added</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5" class="has-text-right">
                            <button class="button" @click="this.$emit('add-material')">Add Material</button>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</template>
<script>
export default {
    name: "EditMaterials",
    props: {
        materials: {
            type: Array,
            required: true
        },
        v: {
            type: Object,
            required: true
        }
    },
    methods: {
        undoMaterial(index) {
            this.$emit('undo-material', index);
        }
    }
}
</script>