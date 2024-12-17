import { QuoteState } from "@/constants/quoteState";

export function getStateClass(state) {
  if (state === QuoteState.ACCEPTED) {
    return 'tag is-success';
  } else if (state === QuoteState.REJECTED) {
    return 'tag is-danger';
  } else if (state === QuoteState.SENT) {
    return 'tag is-info';
  } else {
    return 'tag is-light has-text-black';
  }
}

export function getStateName(state) {
  if (state === QuoteState.ACCEPTED) {
    return 'Accepted';
  } else if (state === QuoteState.REJECTED) {
    return 'Rejected';
  } else if (state === QuoteState.SENT) {
    return 'Sent';
  } else {
    return 'Draft';
  }
}