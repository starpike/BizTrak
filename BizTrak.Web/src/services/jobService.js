import apiClient from "./apiClient";

const jobService = {
    updateJob:
        async (startDate, endDate, job) => {

            let jobId = job.id;

            const patchDocument = [
                { "op": "replace", "path": "/start", "value": startDate.toISOString() },
                { "op": "replace", "path": "/end", "value": endDate.toISOString() },
                { "op": "replace", "path": "/isScheduled", "value": true },
            ];

            try {
                const response = await apiClient.patch(`/jobs/${jobId}`, patchDocument, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                });

                return {
                    ...job,
                    start: startDate,
                    end: endDate,
                    isScheduled: true,
                };
            } catch (error) {
                throw error;
            }
        },
    fetchUnscheduledJobs: async () => {
        try {
            const response = await apiClient.get('/jobs/unscheduled');
            return response.data;
        } catch (error) {
            throw error;
        }
    },
    fetchScheduledJobs: async (start, end) => {
        try {
            const response = await apiClient.get('/jobs/scheduled', {
                params: {
                    start,
                    end
                },
            });

            return response.data.map(j => ({
                id: j.id,
                title: `${j.title} (${j.customerName})`,
                jobTitle: j.title,
                start: j.start,
                end: j.end,
                customerName: j.customerName,
                quoteRef: j.quoteRef,
                quoteId: j.quoteId,
                isScheduled: j.isScheduled
            }));
        } catch (error) {
            throw error;
        }
    }
}

export default jobService;
