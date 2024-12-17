export function createQuote(backendData = null) {
    const defaultQuote = {
        id: null,
        quote_ref: "",
        title: "",
        total_amount: null,
        stateName: "draft",
        state: 0,
        tasks: [],
        materials: [],
        customer_details: null,
    };

    if (backendData) {
        return {
            id: backendData.id || null,
            quote_ref: backendData.quoteRef || "",
            title: backendData.title || "",
            stateName: backendData.stateName || "draft",
            state: backendData.state || 0,
            total_amount: backendData.totalAmount.toFixed(2) || 0.00,
            customer_details: { id: backendData.customerDetail?.id || null, name: backendData.customerDetail?.name || "" },
            tasks: (backendData.tasks ?? []).map(task => ({
                id: task.id || null,
                description: task.desc || task.description || "",
                estimated_duration: task.duration || task.estimatedDuration || 0,
                cost: task.cost.toFixed(2) || 0.00,
                isToBeDeleted: false,
                orderIndex: backendData.orderIndex
            })),
            materials: (backendData.materials ?? []).map(material => ({
                id: material.id || null,
                material_name: material.name || material.materialName || "",
                quantity: material.qty || material.quantity || 0,
                unit_price: material.unitPrice.toFixed(2) || 0.00,
                total_price: material.totalPrice.toFixed(2) || 0.00,
                isToBeDeleted: false
            }))
        };
    }

    return defaultQuote;
}

export function updateQuote(quote, backendData) {
    
    const tasks = (backendData.tasks ?? []).map(task => ({
        id: task.id || null,
        description: task.desc || task.description || "",
        estimated_duration: task.duration || task.estimatedDuration || 0,
        cost: task.cost.toFixed(2) || 0.00,
        isToBeDeleted: false,
        orderIndex: task.orderIndex
    }));

    const materials = (backendData.materials ?? []).map(material => ({
        id: material.id || null,
        material_name: material.name || material.materialName || "",
        quantity: material.qty || material.quantity || 0,
        unit_price: material.unitPrice.toFixed(2) || 0.00,
        total_price: material.totalPrice.toFixed(2) || 0.00,
        isToBeDeleted: false
    }));

    quote.id = backendData.id || null;
    quote.quote_ref = backendData.quoteRef || "";
    quote.title = backendData.title || "";
    quote.stateName = backendData.stateName || "";
    quote.state = backendData.state || 0,
    quote.total_amount = backendData.totalAmount.toFixed(2) || null;
    quote.customer_details = { id: backendData.customerDetail?.id || null, name: backendData.customerDetail?.name || "" };
    quote.tasks = tasks
    quote.materials = materials;
}

export function createPostData(quote) {

    return {
        id: quote.id ?? 0,
        quoteRef: quote.quote_ref,
        title: quote.title,
        state: quote.state,
        totalAmount: quote.total_amount ?? 0,
        customerId: quote.customer_details.id,
        tasks: (quote.tasks ?? [])
            .filter(task => !task.isToBeDeleted)
            .map(task => ({
                id: task.id,
                description: task.desc || task.description,
                estimatedDuration: task.duration || task.estimated_duration,
                cost: task.cost,
                orderIndex: task.orderIndex
            })),
        materials: (quote.materials ?? [])
            .filter(material => !material.isToBeDeleted)
            .map(material => ({
                id: material.id,
                materialName: material.name || material.material_name,
                quantity: material.qty || material.quantity,
                unitPrice: material.unit_price
            }))
    };
}