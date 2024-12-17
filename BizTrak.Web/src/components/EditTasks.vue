<template>
    <div class="field px-3">
        <label class="label">Tasks:</label>
        <div class="control">
            <table class="table responsive-table is-fullwidth is-striped is-hoverable">
                <thead>
                    <tr>
                        <th>
                            <span>Description:</span>
                        </th>
                        <th>
                            <span>Est Duration (hrs):</span>
                        </th>
                        <th>Cost&nbsp;(£)</th>
                        <th>
                            <span>Actions:</span>
                        </th>
                    </tr>
                </thead>
                <draggable :list="tasks" :disabled="!enabled" tag="tbody" item-key="id" class="list-group"
                    ghost-class="ghost" @start="dragging = true" @end="onDragEnd">
                    <template #item="{ element: task, index }">
                        <tr :key="task.id" :class="{ 'not-draggable': !enabled }">
                            <td data-label="Desc:">
                                <input type="text" class="input" v-model="task.description"
                                    :class="{ 'is-danger': v.$each.$response.$data[index].description.$error }" />
                                <span v-if="v.$each.$response.$data[index].description.$error">
                                    <span class="help is-danger"
                                        v-for="error in v.$each.$response.$errors[index].description" :key="error">
                                        {{ error.$message }}
                                    </span>
                                </span>
                            </td>
                            <td data-label="Est (hrs):">
                                <TaskDuration :initial-time="task.estimated_duration"
                                    :v="v.$each.$response.$errors[index].estimated_duration"
                                    @update:duration="(newDuration) => handleDurationUpdate(newDuration, index)">
                                </TaskDuration>
                            </td>
                            <td data-label="Cost (£):">
                                <input type="number" min="0" style="width: 100px;" step="0.01" placeholder="0.00"
                                    class="input" v-model="task.cost"
                                    :class="{ 'is-danger': v.$each.$response.$data[index].cost.$error }" />
                                <span class="help is-danger" v-for="error in v.$each.$response.$errors[index].cost"
                                    :key="error">
                                    {{ error.$message }}
                                </span>
                            </td>
                            <td data-label="" class="is-vtop">
                                <div v-if="task.id">
                                    <label class="checkbox no-wrap">
                                        <input type="checkbox" v-model="task.isToBeDeleted">
                                        <span class="is-hidden-mobile"> Delete</span>
                                        <span class="is-hidden-desktop is-hidden-tablet"> Del</span>
                                    </label>
                                </div>
                                <div v-else>
                                    <button class="button mt-1" @click="undoTask(index)" aria-label="Undo Add"
                                        title="Undo Add">
                                        <i class="fas fa-undo"></i>
                                    </button>
                                </div>
                            </td>
                        </tr>
                    </template>
                    <tr v-if="!tasks || tasks.length === 0">
                        <td colspan="5" class="empty">No tasks added</td>
                    </tr>
                </draggable>
                <tfoot>
                    <tr>
                        <td colspan="4" class="has-text-right">
                            <button class="button" @click="this.$emit('add-task')">Add Task</button>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</template>

<script>
import TaskDuration from './TaskDuration.vue';
import draggable from "vuedraggable";

export default {
    name: "EditTasks",
    data() {
        return {
            dragging: false,
            enabled: true,
        }
    },
    props: {
        tasks: {
            type: Array,
            required: true
        },
        v: {
            type: Object,
            required: true
        }
    },
    components: {
        TaskDuration,
        draggable
    },
    methods: {
        undoTask(index) {
            this.$emit('undo-task', index);
            this.orderTasks();
        },
        handleDurationUpdate(newDuration, index) {
            this.tasks[index].estimated_duration = newDuration;
        },
        onDragEnd(event) {
            this.dragging = false;
            this.orderTasks();
        },
        orderTasks() {
            this.tasks.forEach((task, index) => {
                task.orderIndex = index;
            })
        }
    }
}
</script>
<style scoped>
.buttons {
    margin-top: 35px;
}

.ghost {
    opacity: 0.5;
    background: #c8ebfb;
}

.not-draggable {
    cursor: no-drop;
}
</style>